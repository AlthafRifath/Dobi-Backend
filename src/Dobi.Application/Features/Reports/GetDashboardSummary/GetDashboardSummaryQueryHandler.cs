using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Reports;
using Dobi.Shared.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Reports.GetDashboardSummary
{
    public sealed class GetDashboardSummaryQueryHandler
    : IRequestHandler<GetDashboardSummaryQuery, DashboardSummaryResponse>
    {
        private readonly IDobiDbContext _dbContext;

        public GetDashboardSummaryQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<DashboardSummaryResponse> Handle(
            GetDashboardSummaryQuery request,
            CancellationToken cancellationToken)
        {
            var ordersQuery = _dbContext.Orders
                .AsNoTracking()
                .Include(x => x.CurrentStatus)
                .AsQueryable();

            if (request.FromDate.HasValue)
            {
                var fromDate = request.FromDate.Value.ToDateTime(TimeOnly.MinValue);
                ordersQuery = ordersQuery.Where(x => x.OrderDate >= fromDate);
            }

            if (request.ToDate.HasValue)
            {
                var toDate = request.ToDate.Value.ToDateTime(TimeOnly.MaxValue);
                ordersQuery = ordersQuery.Where(x => x.OrderDate <= toDate);
            }

            var totalOrders = await ordersQuery.CountAsync(cancellationToken);

            var totalOrderValue = await ordersQuery
                .SumAsync(x => x.TotalAmount, cancellationToken);

            var activeOrders = await ordersQuery.CountAsync(x =>
                x.CurrentStatus.StatusCode != OrderStatusCodes.Closed &&
                x.CurrentStatus.StatusCode != OrderStatusCodes.Cancelled,
                cancellationToken);

            var readyForCollectionOrders = await ordersQuery.CountAsync(x =>
                x.CurrentStatus.StatusCode == OrderStatusCodes.ReadyForCollection,
                cancellationToken);

            var collectedDeliveredOrders = await ordersQuery.CountAsync(x =>
                x.CurrentStatus.StatusCode == OrderStatusCodes.CollectedDelivered,
                cancellationToken);

            var closedOrders = await ordersQuery.CountAsync(x =>
                x.CurrentStatus.StatusCode == OrderStatusCodes.Closed,
                cancellationToken);

            var pendingPlantOrders = await ordersQuery.CountAsync(x =>
                x.CurrentStatus.StatusCode == OrderStatusCodes.ReceivedAtPlant ||
                x.CurrentStatus.StatusCode == OrderStatusCodes.Processing ||
                x.CurrentStatus.StatusCode == OrderStatusCodes.QcPending ||
                x.CurrentStatus.StatusCode == OrderStatusCodes.QcFailed ||
                x.CurrentStatus.StatusCode == OrderStatusCodes.Packed ||
                x.CurrentStatus.StatusCode == OrderStatusCodes.ReadyForOutletReturn,
                cancellationToken);

            var paymentsQuery = _dbContext.Payments
                .AsNoTracking()
                .Include(x => x.PaymentStatus)
                .Include(x => x.PaymentMethod)
                .AsQueryable();

            var refundsQuery = _dbContext.Refunds
                .AsNoTracking()
                .Include(x => x.RefundStatus)
                .AsQueryable();

            if (request.FromDate.HasValue)
            {
                var fromDate = request.FromDate.Value.ToDateTime(TimeOnly.MinValue);

                paymentsQuery = paymentsQuery.Where(x =>
                    x.CreatedAt >= fromDate ||
                    (x.PaidAt.HasValue && x.PaidAt.Value >= fromDate));

                refundsQuery = refundsQuery.Where(x =>
                    x.CreatedAt >= fromDate ||
                    (x.RefundedAt.HasValue && x.RefundedAt.Value >= fromDate));
            }

            if (request.ToDate.HasValue)
            {
                var toDate = request.ToDate.Value.ToDateTime(TimeOnly.MaxValue);

                paymentsQuery = paymentsQuery.Where(x =>
                    x.CreatedAt <= toDate ||
                    (x.PaidAt.HasValue && x.PaidAt.Value <= toDate));

                refundsQuery = refundsQuery.Where(x =>
                    x.CreatedAt <= toDate ||
                    (x.RefundedAt.HasValue && x.RefundedAt.Value <= toDate));
            }

            var paidAmount = await paymentsQuery
                .Where(x => x.PaymentStatus.StatusCode == PaymentStatusCodes.Paid)
                .SumAsync(x => x.Amount, cancellationToken);

            var pendingClearanceAmount = await paymentsQuery
                .Where(x => x.PaymentStatus.StatusCode == PaymentStatusCodes.PendingClearance)
                .SumAsync(x => x.Amount, cancellationToken);

            var refundedAmount = await refundsQuery
                .Where(x => x.RefundStatus.StatusCode == RefundStatusCodes.Paid)
                .SumAsync(x => x.Amount, cancellationToken);

            var pendingChequeCount = await paymentsQuery.CountAsync(x =>
                x.PaymentMethod.MethodCode == PaymentMethodCodes.Cheque &&
                x.PaymentStatus.StatusCode == PaymentStatusCodes.PendingClearance,
                cancellationToken);

            var pendingChequeAmount = await paymentsQuery
                .Where(x =>
                    x.PaymentMethod.MethodCode == PaymentMethodCodes.Cheque &&
                    x.PaymentStatus.StatusCode == PaymentStatusCodes.PendingClearance)
                .SumAsync(x => x.Amount, cancellationToken);

            return new DashboardSummaryResponse(
                totalOrders,
                activeOrders,
                readyForCollectionOrders,
                collectedDeliveredOrders,
                closedOrders,
                pendingPlantOrders,
                totalOrderValue,
                paidAmount,
                pendingClearanceAmount,
                refundedAmount,
                paidAmount - refundedAmount,
                pendingChequeCount,
                pendingChequeAmount);
        }
    }
}
