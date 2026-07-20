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

namespace Dobi.Application.Features.Orders.GetEligiblePlantTransferOrders
{
    public sealed class GetEligiblePlantTransferOrdersQueryHandler : IRequestHandler<GetEligiblePlantTransferOrdersQuery, PagedResponse<EligiblePlantTransferOrderResponse>>
    {
        private readonly IDobiDbContext _dbContext;

        public GetEligiblePlantTransferOrdersQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResponse<EligiblePlantTransferOrderResponse>> Handle(
            GetEligiblePlantTransferOrdersQuery request,
            CancellationToken cancellationToken)
        {
            var sourceBranch = await _dbContext.Branches
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.FromBranchId, cancellationToken);

            if (sourceBranch is null)
            {
                throw new NotFoundException("Source branch", request.FromBranchId);
            }

            if (!sourceBranch.IsActive)
            {
                throw new BadRequestException("Source branch is inactive.");
            }

            var destinationPlant = await _dbContext.Plants
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.ToPlantId, cancellationToken);

            if (destinationPlant is null)
            {
                throw new NotFoundException("Destination plant", request.ToPlantId);
            }

            if (!destinationPlant.IsActive)
            {
                throw new BadRequestException("Destination plant is inactive.");
            }

            var receivedTransferStatus = await _dbContext.TransferStatuses
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.StatusCode == TransferStatusCodes.Received, cancellationToken);

            if (receivedTransferStatus is null)
            {
                throw new InvalidOperationException("Transfer status 'RECEIVED' was not found.");
            }

            var outletToPlantTransferType = await _dbContext.TransferTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.TransferTypeCode == TransferTypeCodes.OutletToPlant, cancellationToken);

            if (outletToPlantTransferType is null)
            {
                throw new InvalidOperationException("Transfer type 'OUTLET_TO_PLANT' was not found.");
            }

            var activeInboundTransferOrderIdsQuery =
                from transferItem in _dbContext.TransferBatchItems.AsNoTracking()
                join transferBatch in _dbContext.TransferBatches.AsNoTracking()
                    on transferItem.TransferBatchId equals transferBatch.Id
                where transferBatch.TransferTypeId == outletToPlantTransferType.Id
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
                .Where(x =>
                    x.BranchId == request.FromBranchId &&
                    x.CurrentStatus.StatusCode == OrderStatusCodes.Created &&
                    !activeInboundTransferOrderIdsQuery.Contains(x.Id))
                .AsQueryable();

            if (request.CustomerTypeId.HasValue)
            {
                query = query.Where(x => x.Customer.CustomerTypeId == request.CustomerTypeId.Value);
            }

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
                .OrderByDescending(x => x.OrderDate)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToArrayAsync(cancellationToken);

            var items = orders
                .Select(order => new EligiblePlantTransferOrderResponse
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

                    SourceBranchId = order.BranchId,
                    SourceBranchName = order.Branch.BranchName,

                    DestinationPlantId = destinationPlant.Id,
                    DestinationPlantName = destinationPlant.PlantName,

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

            return new PagedResponse<EligiblePlantTransferOrderResponse>(
                items,
                totalCount,
                request.PageNumber,
                request.PageSize,
                totalPages,
                request.PageNumber > 1,
                request.PageNumber < totalPages);
        }
    }
}
