using Dobi.Application.Abstractions.Persistence;
using Dobi.Application.Common;
using Dobi.Contracts.Common;
using Dobi.Contracts.Orders;
using Dobi.Shared.Constants;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Orders.GetEligibleOutletReturnOrders
{
    public sealed class GetEligibleOutletReturnOrdersQueryHandler : IRequestHandler<GetEligibleOutletReturnOrdersQuery, PagedResponse<EligibleOutletReturnOrderResponse>>
    {
        private readonly IDobiDbContext _dbContext;

        public GetEligibleOutletReturnOrdersQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResponse<EligibleOutletReturnOrderResponse>> Handle(
            GetEligibleOutletReturnOrdersQuery request,
            CancellationToken cancellationToken)
        {
            var sourcePlant = await _dbContext.Plants
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.FromPlantId, cancellationToken);

            if (sourcePlant is null)
            {
                throw new NotFoundException("Source plant", request.FromPlantId);
            }

            if (!sourcePlant.IsActive)
            {
                throw new BadRequestException("Source plant is inactive.");
            }

            var destinationBranch = await _dbContext.Branches
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.ToBranchId, cancellationToken);

            if (destinationBranch is null)
            {
                throw new NotFoundException("Destination branch", request.ToBranchId);
            }

            if (!destinationBranch.IsActive)
            {
                throw new BadRequestException("Destination branch is inactive.");
            }

            var receivedTransferStatus = await _dbContext.TransferStatuses
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.StatusCode == TransferStatusCodes.Received, cancellationToken);

            if (receivedTransferStatus is null)
            {
                throw new InvalidOperationException("Transfer status 'RECEIVED' was not found.");
            }

            var plantToOutletTransferType = await _dbContext.TransferTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.TransferTypeCode == TransferTypeCodes.PlantToOutlet, cancellationToken);

            if (plantToOutletTransferType is null)
            {
                throw new InvalidOperationException("Transfer type 'PLANT_TO_OUTLET' was not found.");
            }

            var activeReturnTransferOrderIdsQuery =
                from transferItem in _dbContext.TransferBatchItems.AsNoTracking()
                join transferBatch in _dbContext.TransferBatches.AsNoTracking()
                    on transferItem.TransferBatchId equals transferBatch.Id
                where transferBatch.TransferTypeId == plantToOutletTransferType.Id
                      && transferBatch.TransferStatusId != receivedTransferStatus.Id
                select transferItem.OrderId;

            var query = _dbContext.Orders
                .AsNoTracking()
                .Include(x => x.Customer)
                    .ThenInclude(x => x.CustomerType)
                .Include(x => x.Branch)
                .Include(x => x.CurrentStatus)
                .Include(x => x.PaymentStatus)
                .Include(x => x.Items)
                .Include(x => x.PlantProcessing)
                    .ThenInclude(x => x!.Plant)
                .Where(x =>
                    x.BranchId == request.ToBranchId &&
                    x.PlantProcessing != null &&
                    x.PlantProcessing.PlantId == request.FromPlantId &&
                    x.PlantProcessing.ReadyDate.HasValue &&
                    x.CurrentStatus.StatusCode == OrderStatusCodes.ReadyForOutletReturn &&
                    x.Customer.CustomerType.CustomerTypeCode == CustomerTypeCodes.B2C &&
                    !activeReturnTransferOrderIdsQuery.Contains(x.Id))
                .AsQueryable();

            if (request.ExcludeOrderIds.Count > 0)
            {
                query = query.Where(x => !request.ExcludeOrderIds.Contains(x.Id));
            }

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.Trim().ToLower();

                query = query.Where(x =>
                    x.OrderNo.ToLower().Contains(searchTerm) ||
                    x.Customer.FullName.ToLower().Contains(searchTerm) ||
                    x.Customer.MobileNo.ToLower().Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var orders = await query
                .OrderBy(x => x.PlantProcessing!.ReadyDate)
                .ThenBy(x => x.OrderNo)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToArrayAsync(cancellationToken);

            var items = orders
                .Select(order => new EligibleOutletReturnOrderResponse
                {
                    OrderId = order.Id,
                    OrderNo = order.OrderNo,
                    OrderDate = order.OrderDate,

                    CustomerId = order.CustomerId,
                    CustomerName = order.Customer.FullName,
                    CustomerMobileNo = order.Customer.MobileNo,

                    CustomerTypeId = order.Customer.CustomerTypeId,
                    CustomerTypeCode = LookupValueHelper.GetCode(order.Customer.CustomerType),
                    CustomerTypeName = LookupValueHelper.GetName(order.Customer.CustomerType),

                    SourcePlantId = order.PlantProcessing!.PlantId,
                    SourcePlantName = order.PlantProcessing.Plant.PlantName,

                    DestinationBranchId = order.BranchId,
                    DestinationBranchName = order.Branch.BranchName,

                    PlantProcessingId = order.PlantProcessing.Id,
                    ReadyForOutletReturnAt = ToUtcDateTime(order.PlantProcessing.ReadyDate),

                    CurrentOrderStatusId = order.CurrentStatusId,
                    CurrentOrderStatusCode = LookupValueHelper.GetCode(order.CurrentStatus),
                    CurrentOrderStatusName = LookupValueHelper.GetName(order.CurrentStatus),

                    PaymentStatusId = order.PaymentStatusId,
                    PaymentStatusCode = LookupValueHelper.GetCode(order.PaymentStatus),
                    PaymentStatusName = LookupValueHelper.GetName(order.PaymentStatus),

                    ItemCount = order.Items.Count,
                    TotalQuantity = order.Items.Sum(x => (decimal)x.Quantity),
                    TotalAmount = order.TotalAmount
                })
                .ToArray();

            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            return new PagedResponse<EligibleOutletReturnOrderResponse>(
                items,
                totalCount,
                request.PageNumber,
                request.PageSize,
                totalPages,
                request.PageNumber > 1,
                request.PageNumber < totalPages);
        }

        private static DateTime? ToUtcDateTime(DateOnly? date)
        {
            if (!date.HasValue)
            {
                return null;
            }

            return DateTime.SpecifyKind(
                date.Value.ToDateTime(TimeOnly.MinValue),
                DateTimeKind.Utc);
        }
    }
}
