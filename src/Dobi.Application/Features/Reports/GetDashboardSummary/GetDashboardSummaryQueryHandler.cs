using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Reports;
using Dobi.Shared.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dobi.Application.Features.Reports.GetDashboardSummary;

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
        /*
         * DateOnly.ToDateTime normally creates DateTimeKind.Unspecified.
         * PostgreSQL timestamp with time zone requires UTC DateTime values.
         *
         * ToDate is converted to the beginning of the following day and used
         * as an exclusive upper boundary:
         *
         * FromDate <= timestamp < ToDate + 1 day
         */
        DateTime? fromDateUtc = request.FromDate.HasValue
            ? request.FromDate.Value.ToDateTime(
                TimeOnly.MinValue,
                DateTimeKind.Utc)
            : null;

        DateTime? toDateExclusiveUtc = request.ToDate.HasValue
            ? request.ToDate.Value
                .AddDays(1)
                .ToDateTime(
                    TimeOnly.MinValue,
                    DateTimeKind.Utc)
            : null;

        var ordersQuery = _dbContext.Orders
            .AsNoTracking()
            .AsQueryable();

        if (fromDateUtc.HasValue)
        {
            var fromDate = fromDateUtc.Value;

            ordersQuery = ordersQuery.Where(x =>
                x.OrderDate >= fromDate);
        }

        if (toDateExclusiveUtc.HasValue)
        {
            var toDateExclusive = toDateExclusiveUtc.Value;

            ordersQuery = ordersQuery.Where(x =>
                x.OrderDate < toDateExclusive);
        }

        var totalOrders = await ordersQuery
            .CountAsync(cancellationToken);

        var totalOrderValue = await ordersQuery
            .SumAsync(
                x => x.TotalAmount,
                cancellationToken);

        var activeOrders = await ordersQuery
            .CountAsync(
                x =>
                    x.CurrentStatus.StatusCode != OrderStatusCodes.Closed &&
                    x.CurrentStatus.StatusCode != OrderStatusCodes.Cancelled,
                cancellationToken);

        var readyForCollectionOrders = await ordersQuery
            .CountAsync(
                x => x.CurrentStatus.StatusCode ==
                     OrderStatusCodes.ReadyForCollection,
                cancellationToken);

        var collectedDeliveredOrders = await ordersQuery
            .CountAsync(
                x => x.CurrentStatus.StatusCode ==
                     OrderStatusCodes.CollectedDelivered,
                cancellationToken);

        var closedOrders = await ordersQuery
            .CountAsync(
                x => x.CurrentStatus.StatusCode ==
                     OrderStatusCodes.Closed,
                cancellationToken);

        var pendingPlantOrders = await ordersQuery
            .CountAsync(
                x =>
                    x.CurrentStatus.StatusCode ==
                        OrderStatusCodes.ReceivedAtPlant ||
                    x.CurrentStatus.StatusCode ==
                        OrderStatusCodes.Processing ||
                    x.CurrentStatus.StatusCode ==
                        OrderStatusCodes.QcPending ||
                    x.CurrentStatus.StatusCode ==
                        OrderStatusCodes.QcFailed ||
                    x.CurrentStatus.StatusCode ==
                        OrderStatusCodes.Packed ||
                    x.CurrentStatus.StatusCode ==
                        OrderStatusCodes.ReadyForOutletReturn,
                cancellationToken);

        var paymentsQuery = _dbContext.Payments
            .AsNoTracking()
            .AsQueryable();

        var refundsQuery = _dbContext.Refunds
            .AsNoTracking()
            .AsQueryable();

        if (fromDateUtc.HasValue)
        {
            var fromDate = fromDateUtc.Value;

            paymentsQuery = paymentsQuery.Where(x =>
                x.CreatedAt >= fromDate ||
                (x.PaidAt.HasValue &&
                 x.PaidAt.Value >= fromDate));

            refundsQuery = refundsQuery.Where(x =>
                x.CreatedAt >= fromDate ||
                (x.RefundedAt.HasValue &&
                 x.RefundedAt.Value >= fromDate));
        }

        if (toDateExclusiveUtc.HasValue)
        {
            var toDateExclusive = toDateExclusiveUtc.Value;

            paymentsQuery = paymentsQuery.Where(x =>
                x.CreatedAt < toDateExclusive ||
                (x.PaidAt.HasValue &&
                 x.PaidAt.Value < toDateExclusive));

            refundsQuery = refundsQuery.Where(x =>
                x.CreatedAt < toDateExclusive ||
                (x.RefundedAt.HasValue &&
                 x.RefundedAt.Value < toDateExclusive));
        }

        var paidAmount = await paymentsQuery
            .Where(x =>
                x.PaymentStatus.StatusCode ==
                PaymentStatusCodes.Paid)
            .SumAsync(
                x => x.Amount,
                cancellationToken);

        var pendingClearanceAmount = await paymentsQuery
            .Where(x =>
                x.PaymentStatus.StatusCode ==
                PaymentStatusCodes.PendingClearance)
            .SumAsync(
                x => x.Amount,
                cancellationToken);

        var refundedAmount = await refundsQuery
            .Where(x =>
                x.RefundStatus.StatusCode ==
                RefundStatusCodes.Paid)
            .SumAsync(
                x => x.Amount,
                cancellationToken);

        var pendingChequeCount = await paymentsQuery
            .CountAsync(
                x =>
                    x.PaymentMethod.MethodCode ==
                        PaymentMethodCodes.Cheque &&
                    x.PaymentStatus.StatusCode ==
                        PaymentStatusCodes.PendingClearance,
                cancellationToken);

        var pendingChequeAmount = await paymentsQuery
            .Where(x =>
                x.PaymentMethod.MethodCode ==
                    PaymentMethodCodes.Cheque &&
                x.PaymentStatus.StatusCode ==
                    PaymentStatusCodes.PendingClearance)
            .SumAsync(
                x => x.Amount,
                cancellationToken);

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