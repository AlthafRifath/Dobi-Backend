using Dobi.Application.Abstractions.Persistence;
using Dobi.Application.Common;
using Dobi.Contracts.Reports;
using Dobi.Shared.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dobi.Application.Features.Reports.GetDailyRevenueReport;

public sealed class GetDailyRevenueReportQueryHandler
    : IRequestHandler<
        GetDailyRevenueReportQuery,
        IReadOnlyCollection<DailyRevenueReportResponse>>
{
    private readonly IDobiDbContext _dbContext;

    public GetDailyRevenueReportQueryHandler(IDobiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<DailyRevenueReportResponse>> Handle(
        GetDailyRevenueReportQuery request,
        CancellationToken cancellationToken)
    {
        var fromDateTimeUtc = request.FromDate.ToDateTime(
            TimeOnly.MinValue,
            DateTimeKind.Utc);

        var toDateTimeExclusiveUtc = request.ToDate
            .AddDays(1)
            .ToDateTime(
                TimeOnly.MinValue,
                DateTimeKind.Utc);

        var paidPayments = await _dbContext.Payments
            .AsNoTracking()
            .Include(x => x.PaymentMethod)
            .Include(x => x.PaymentStatus)
            .Where(x =>
                x.PaymentStatus.StatusCode == PaymentStatusCodes.Paid &&
                x.PaidAt.HasValue &&
                x.PaidAt.Value >= fromDateTimeUtc &&
                x.PaidAt.Value < toDateTimeExclusiveUtc)
            .ToArrayAsync(cancellationToken);

        var paidRefunds = await _dbContext.Refunds
            .AsNoTracking()
            .Include(x => x.RefundStatus)
            .Where(x =>
                x.RefundStatus.StatusCode == RefundStatusCodes.Paid &&
                x.RefundedAt.HasValue &&
                x.RefundedAt.Value >= fromDateTimeUtc &&
                x.RefundedAt.Value < toDateTimeExclusiveUtc)
            .ToArrayAsync(cancellationToken);

        var result = new List<DailyRevenueReportResponse>();

        for (var date = request.FromDate;
             date <= request.ToDate;
             date = date.AddDays(1))
        {
            var dayPayments = paidPayments
                .Where(x =>
                    DateOnly.FromDateTime(x.PaidAt!.Value) == date)
                .ToArray();

            var cashAmount = dayPayments
                .Where(x =>
                    LookupValueHelper.GetCode(x.PaymentMethod) ==
                    PaymentMethodCodes.Cash)
                .Sum(x => x.Amount);

            var cardAmount = dayPayments
                .Where(x =>
                    LookupValueHelper.GetCode(x.PaymentMethod) ==
                    PaymentMethodCodes.Card)
                .Sum(x => x.Amount);

            var bankTransferAmount = dayPayments
                .Where(x =>
                    LookupValueHelper.GetCode(x.PaymentMethod) ==
                    PaymentMethodCodes.BankTransfer)
                .Sum(x => x.Amount);

            var chequeClearedAmount = dayPayments
                .Where(x =>
                    LookupValueHelper.GetCode(x.PaymentMethod) ==
                    PaymentMethodCodes.Cheque)
                .Sum(x => x.Amount);

            var totalPaidAmount =
                cashAmount +
                cardAmount +
                bankTransferAmount +
                chequeClearedAmount;

            var refundedAmount = paidRefunds
                .Where(x =>
                    DateOnly.FromDateTime(x.RefundedAt!.Value) == date)
                .Sum(x => x.Amount);

            result.Add(new DailyRevenueReportResponse(
                date,
                cashAmount,
                cardAmount,
                bankTransferAmount,
                chequeClearedAmount,
                totalPaidAmount,
                refundedAmount,
                totalPaidAmount - refundedAmount));
        }

        return result;
    }
}