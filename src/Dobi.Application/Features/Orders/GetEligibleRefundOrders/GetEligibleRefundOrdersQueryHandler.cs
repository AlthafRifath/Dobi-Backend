using Dobi.Application.Abstractions.Persistence;
using Dobi.Application.Common;
using Dobi.Contracts.Common;
using Dobi.Contracts.Orders;
using Dobi.Shared.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Orders.GetEligibleRefundOrders
{
    public sealed class GetEligibleRefundOrdersQueryHandler : IRequestHandler<GetEligibleRefundOrdersQuery, PagedResponse<EligibleRefundOrderResponse>>
    {
        private readonly IDobiDbContext _dbContext;

        public GetEligibleRefundOrdersQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResponse<EligibleRefundOrderResponse>> Handle(
            GetEligibleRefundOrdersQuery request,
            CancellationToken cancellationToken)
        {
            var paidPaymentStatusId = await GetPaymentStatusIdByCodeAsync(
                PaymentStatusCodes.Paid,
                cancellationToken);

            var pendingRefundStatusId = await GetRefundStatusIdByCodeAsync(
                RefundStatusCodes.Pending,
                cancellationToken);

            var approvedRefundStatusId = await GetRefundStatusIdByCodeAsync(
                RefundStatusCodes.Approved,
                cancellationToken);

            var paidRefundStatusId = await GetRefundStatusIdByCodeAsync(
                RefundStatusCodes.Paid,
                cancellationToken);

            var paidOrderIdsQuery = _dbContext.Payments
                .AsNoTracking()
                .Where(x => x.PaymentStatusId == paidPaymentStatusId)
                .Select(x => x.OrderId)
                .Distinct();

            var query = _dbContext.Orders
                .AsNoTracking()
                .Include(x => x.Customer)
                    .ThenInclude(x => x.CustomerType)
                .Include(x => x.Branch)
                .Include(x => x.CurrentStatus)
                .Include(x => x.PaymentStatus)
                .Include(x => x.Items)
                .Where(x =>
                    paidOrderIdsQuery.Contains(x.Id) &&
                    x.CurrentStatus.StatusCode != OrderStatusCodes.Cancelled &&
                    x.PaymentStatus.StatusCode != PaymentStatusCodes.Refunded)
                .AsQueryable();

            if (request.CustomerId.HasValue)
            {
                query = query.Where(x => x.CustomerId == request.CustomerId.Value);
            }

            if (request.BranchId.HasValue)
            {
                query = query.Where(x => x.BranchId == request.BranchId.Value);
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
                .OrderByDescending(x => x.OrderDate)
                .ThenBy(x => x.OrderNo)
                .ToArrayAsync(cancellationToken);

            var eligibleOrders = new List<EligibleRefundOrderResponse>();

            foreach (var order in candidateOrders)
            {
                var clearedPaidAmount = await _dbContext.Payments
                    .AsNoTracking()
                    .Where(x =>
                        x.OrderId == order.Id &&
                        x.PaymentStatusId == paidPaymentStatusId)
                    .SumAsync(x => x.Amount, cancellationToken);

                if (clearedPaidAmount <= 0)
                {
                    continue;
                }

                var pendingRefundAmount = await _dbContext.Refunds
                    .AsNoTracking()
                    .Where(x =>
                        x.OrderId == order.Id &&
                        x.RefundStatusId == pendingRefundStatusId)
                    .SumAsync(x => x.Amount, cancellationToken);

                var approvedPendingRefundAmount = await _dbContext.Refunds
                    .AsNoTracking()
                    .Where(x =>
                        x.OrderId == order.Id &&
                        x.RefundStatusId == approvedRefundStatusId)
                    .SumAsync(x => x.Amount, cancellationToken);

                var completedRefundAmount = await _dbContext.Refunds
                    .AsNoTracking()
                    .Where(x =>
                        x.OrderId == order.Id &&
                        x.RefundStatusId == paidRefundStatusId)
                    .SumAsync(x => x.Amount, cancellationToken);

                var reservedRefundAmount =
                    pendingRefundAmount +
                    approvedPendingRefundAmount +
                    completedRefundAmount;

                var availableRefundAmount = Math.Max(
                    clearedPaidAmount - reservedRefundAmount,
                    0);

                if (availableRefundAmount <= 0)
                {
                    continue;
                }

                eligibleOrders.Add(new EligibleRefundOrderResponse
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

                    CurrentOrderStatusId = order.CurrentStatusId,
                    CurrentOrderStatusCode = LookupValueHelper.GetCode(order.CurrentStatus),
                    CurrentOrderStatusName = LookupValueHelper.GetName(order.CurrentStatus),

                    PaymentStatusId = order.PaymentStatusId,
                    PaymentStatusCode = LookupValueHelper.GetCode(order.PaymentStatus),
                    PaymentStatusName = LookupValueHelper.GetName(order.PaymentStatus),

                    ItemCount = order.Items.Count,
                    TotalQuantity = order.Items.Sum(x => (decimal)x.Quantity),
                    TotalAmount = order.TotalAmount,

                    OrderTotalAmount = order.TotalAmount,

                    ClearedPaidAmount = clearedPaidAmount,

                    PendingRefundAmount = pendingRefundAmount,
                    ApprovedPendingRefundAmount = approvedPendingRefundAmount,
                    CompletedRefundAmount = completedRefundAmount,

                    ReservedRefundAmount = reservedRefundAmount,
                    AvailableRefundAmount = availableRefundAmount,

                    // We do not have a separate company-side issue confirmation field yet.
                    // For MVP, the authorized user confirms the reason during refund creation/approval.
                    HasConfirmedCompanyIssue = true
                });
            }

            var totalCount = eligibleOrders.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            var pagedItems = eligibleOrders
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToArray();

            return new PagedResponse<EligibleRefundOrderResponse>(
                pagedItems,
                totalCount,
                request.PageNumber,
                request.PageSize,
                totalPages,
                request.PageNumber > 1,
                request.PageNumber < totalPages);
        }

        private async Task<int> GetPaymentStatusIdByCodeAsync(
            string statusCode,
            CancellationToken cancellationToken)
        {
            var statuses = await _dbContext.PaymentStatuses
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var status = statuses.FirstOrDefault(x =>
                LookupValueHelper.GetCode(x) == statusCode);

            if (status is null)
            {
                throw new InvalidOperationException($"Payment status '{statusCode}' was not found.");
            }

            return status.Id;
        }

        private async Task<int> GetRefundStatusIdByCodeAsync(
            string statusCode,
            CancellationToken cancellationToken)
        {
            var statuses = await _dbContext.RefundStatuses
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var status = statuses.FirstOrDefault(x =>
                LookupValueHelper.GetCode(x) == statusCode);

            if (status is null)
            {
                throw new InvalidOperationException($"Refund status '{statusCode}' was not found.");
            }

            return status.Id;
        }
    }
}
