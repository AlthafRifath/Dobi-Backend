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

namespace Dobi.Application.Features.Orders.GetEligiblePlantProcessingOrders
{
    public sealed class GetEligiblePlantProcessingOrdersQueryHandler : IRequestHandler<GetEligiblePlantProcessingOrdersQuery, PagedResponse<EligiblePlantProcessingOrderResponse>>
    {
        private readonly IDobiDbContext _dbContext;

        public GetEligiblePlantProcessingOrdersQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResponse<EligiblePlantProcessingOrderResponse>> Handle(
            GetEligiblePlantProcessingOrdersQuery request,
            CancellationToken cancellationToken)
        {
            var plant = await _dbContext.Plants
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.PlantId, cancellationToken);

            if (plant is null)
            {
                throw new NotFoundException("Plant", request.PlantId);
            }

            if (!plant.IsActive)
            {
                throw new BadRequestException("Plant is inactive.");
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

            var receivedTransferRowsQuery =
                from transferItem in _dbContext.TransferBatchItems.AsNoTracking()
                join transferBatch in _dbContext.TransferBatches.AsNoTracking()
                    on transferItem.TransferBatchId equals transferBatch.Id
                where transferBatch.TransferTypeId == outletToPlantTransferType.Id
                      && transferBatch.TransferStatusId == receivedTransferStatus.Id
                      && transferBatch.ToPlantId == request.PlantId
                select new
                {
                    transferItem.OrderId,
                    TransferBatchId = transferBatch.Id,
                    transferBatch.TransferNo,
                    transferBatch.ReceivedAt
                };

            var activeInboundTransferOrderIdsQuery =
                from transferItem in _dbContext.TransferBatchItems.AsNoTracking()
                join transferBatch in _dbContext.TransferBatches.AsNoTracking()
                    on transferItem.TransferBatchId equals transferBatch.Id
                where transferBatch.TransferTypeId == outletToPlantTransferType.Id
                      && transferBatch.TransferStatusId != receivedTransferStatus.Id
                select transferItem.OrderId;

            var query =
                from order in _dbContext.Orders
                    .AsNoTracking()
                    .Include(x => x.Customer)
                        .ThenInclude(x => x.CustomerType)
                    .Include(x => x.Branch)
                    .Include(x => x.CurrentStatus)
                    .Include(x => x.PaymentStatus)
                    .Include(x => x.Items)
                join receivedTransfer in receivedTransferRowsQuery
                    on order.Id equals receivedTransfer.OrderId
                where order.CurrentStatus.StatusCode == OrderStatusCodes.ReceivedAtPlant
                      && order.PlantProcessing == null
                      && !activeInboundTransferOrderIdsQuery.Contains(order.Id)
                select new
                {
                    Order = order,
                    receivedTransfer.TransferBatchId,
                    receivedTransfer.TransferNo,
                    receivedTransfer.ReceivedAt
                };

            if (request.CustomerId.HasValue)
            {
                query = query.Where(x => x.Order.CustomerId == request.CustomerId.Value);
            }

            if (request.BranchId.HasValue)
            {
                query = query.Where(x => x.Order.BranchId == request.BranchId.Value);
            }

            if (request.CustomerTypeId.HasValue)
            {
                query = query.Where(x => x.Order.Customer.CustomerTypeId == request.CustomerTypeId.Value);
            }

            if (request.ExcludeOrderIds.Count > 0)
            {
                query = query.Where(x => !request.ExcludeOrderIds.Contains(x.Order.Id));
            }

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.Trim().ToLower();

                query = query.Where(x =>
                    x.Order.OrderNo.ToLower().Contains(searchTerm) ||
                    x.Order.Customer.FullName.ToLower().Contains(searchTerm) ||
                    x.Order.Customer.MobileNo.ToLower().Contains(searchTerm));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var records = await query
                .OrderBy(x => x.ReceivedAt)
                .ThenBy(x => x.Order.OrderNo)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToArrayAsync(cancellationToken);

            var items = records
                .Select(record => new EligiblePlantProcessingOrderResponse
                {
                    OrderId = record.Order.Id,
                    OrderNo = record.Order.OrderNo,
                    OrderDate = record.Order.OrderDate,

                    CustomerId = record.Order.CustomerId,
                    CustomerName = record.Order.Customer.FullName,
                    CustomerMobileNo = record.Order.Customer.MobileNo,

                    CustomerTypeId = record.Order.Customer.CustomerTypeId,
                    CustomerTypeCode = LookupValueHelper.GetCode(record.Order.Customer.CustomerType),
                    CustomerTypeName = LookupValueHelper.GetName(record.Order.Customer.CustomerType),

                    BranchId = record.Order.BranchId,
                    BranchName = record.Order.Branch.BranchName,

                    PlantId = plant.Id,
                    PlantName = plant.PlantName,

                    ReceivedTransferBatchId = record.TransferBatchId,
                    ReceivedTransferNo = record.TransferNo,
                    ReceivedAtPlantAt = record.ReceivedAt,

                    CurrentOrderStatusId = record.Order.CurrentStatusId,
                    CurrentOrderStatusCode = LookupValueHelper.GetCode(record.Order.CurrentStatus),
                    CurrentOrderStatusName = LookupValueHelper.GetName(record.Order.CurrentStatus),

                    PaymentStatusId = record.Order.PaymentStatusId,
                    PaymentStatusCode = LookupValueHelper.GetCode(record.Order.PaymentStatus),
                    PaymentStatusName = LookupValueHelper.GetName(record.Order.PaymentStatus),

                    ItemCount = record.Order.Items.Count,
                    TotalQuantity = record.Order.Items.Sum(x => (decimal)x.Quantity),
                    TotalAmount = record.Order.TotalAmount
                })
                .ToArray();

            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            return new PagedResponse<EligiblePlantProcessingOrderResponse>(
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
