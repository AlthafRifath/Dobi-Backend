using Dobi.Application.Abstractions.Persistence;
using Dobi.Application.Common;
using Dobi.Application.Features.Payments;
using Dobi.Contracts.Common;
using Dobi.Contracts.Orders;
using Dobi.Shared.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Orders.GetEligiblePaymentOrders
{
    public sealed class GetEligiblePaymentOrdersQueryHandler : IRequestHandler<GetEligiblePaymentOrdersQuery, PagedResponse<EligiblePaymentOrderResponse>>
    {
        private readonly IDobiDbContext _dbContext;

        public GetEligiblePaymentOrdersQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResponse<EligiblePaymentOrderResponse>> Handle(
            GetEligiblePaymentOrdersQuery request,
            CancellationToken cancellationToken)
        {
            var query = _dbContext.Orders
                .AsNoTracking()
                .Include(x => x.Customer)
                    .ThenInclude(x => x.CustomerType)
                .Include(x => x.Branch)
                .Include(x => x.CurrentStatus)
                .Include(x => x.PaymentStatus)
                .Include(x => x.Items)
                .Include(x => x.Collection)
                    .ThenInclude(x => x!.CollectionMode)
                .Where(x =>
                    x.CurrentStatus.StatusCode == OrderStatusCodes.CollectedDelivered &&
                    x.PaymentStatus.StatusCode != PaymentStatusCodes.Paid &&
                    x.PaymentStatus.StatusCode != PaymentStatusCodes.Refunded &&
                    x.PaymentStatus.StatusCode != PaymentStatusCodes.Failed)
                .AsQueryable();

            if (request.BranchId.HasValue)
            {
                query = query.Where(x => x.BranchId == request.BranchId.Value);
            }

            if (request.CustomerId.HasValue)
            {
                query = query.Where(x => x.CustomerId == request.CustomerId.Value);
            }

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

            var candidateOrders = await query
                .OrderByDescending(x => x.Collection == null
                    ? x.OrderDate
                    : x.Collection.CollectedOrDeliveredAt)
                .ThenBy(x => x.OrderNo)
                .ToArrayAsync(cancellationToken);

            var eligibleOrders = new List<EligiblePaymentOrderResponse>();

            foreach (var order in candidateOrders)
            {
                var summary = await PaymentStatusUpdater.GetSummaryAsync(
                    _dbContext,
                    order,
                    cancellationToken);

                if (summary.OutstandingAmount <= 0)
                {
                    continue;
                }

                eligibleOrders.Add(new EligiblePaymentOrderResponse
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

                    BranchId = order.BranchId,
                    BranchName = order.Branch.BranchName,

                    CollectionId = order.Collection?.Id,
                    CollectionModeId = order.Collection?.CollectionModeId,
                    CollectionModeCode = order.Collection?.CollectionMode is null
                        ? null
                        : LookupValueHelper.GetCode(order.Collection.CollectionMode),
                    CollectionModeName = order.Collection?.CollectionMode is null
                        ? null
                        : LookupValueHelper.GetName(order.Collection.CollectionMode),
                    CollectedOrDeliveredAt = order.Collection?.CollectedOrDeliveredAt,

                    CurrentOrderStatusId = order.CurrentStatusId,
                    CurrentOrderStatusCode = LookupValueHelper.GetCode(order.CurrentStatus),
                    CurrentOrderStatusName = LookupValueHelper.GetName(order.CurrentStatus),

                    PaymentStatusId = order.PaymentStatusId,
                    PaymentStatusCode = summary.OrderPaymentStatusCode,
                    PaymentStatusName = summary.OrderPaymentStatusName,

                    ItemCount = order.Items.Count,
                    TotalQuantity = order.Items.Sum(x => (decimal)x.Quantity),
                    TotalAmount = order.TotalAmount,

                    OrderTotalAmount = summary.OrderTotalAmount,
                    PaidAmount = summary.PaidAmount,
                    PendingClearanceAmount = summary.PendingClearanceAmount,
                    FailedAmount = summary.FailedAmount,
                    OutstandingAmount = summary.OutstandingAmount
                });
            }

            var totalCount = eligibleOrders.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            var pagedItems = eligibleOrders
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToArray();

            return new PagedResponse<EligiblePaymentOrderResponse>(
                pagedItems,
                totalCount,
                request.PageNumber,
                request.PageSize,
                totalPages,
                request.PageNumber > 1,
                request.PageNumber < totalPages);
        }
    }
}
