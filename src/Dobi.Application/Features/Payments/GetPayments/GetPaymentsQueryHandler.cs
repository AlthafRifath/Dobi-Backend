using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Common;
using Dobi.Contracts.Payments;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dobi.Application.Features.Payments.GetPayments;

public sealed class GetPaymentsQueryHandler
    : IRequestHandler<GetPaymentsQuery, PagedResponse<PaymentResponse>>
{
    private readonly IDobiDbContext _dbContext;

    public GetPaymentsQueryHandler(IDobiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResponse<PaymentResponse>> Handle(
        GetPaymentsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.Payments
            .AsNoTracking()
            .Include(x => x.Order)
            .Include(x => x.PaymentMethod)
            .Include(x => x.PaymentStatus)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm
                .Trim()
                .ToLower();

            query = query.Where(x =>
                x.Order.OrderNo.ToLower().Contains(searchTerm) ||
                (
                    x.ReferenceNo != null &&
                    x.ReferenceNo.ToLower().Contains(searchTerm)
                ) ||
                (
                    x.ChequeNo != null &&
                    x.ChequeNo.ToLower().Contains(searchTerm)
                ) ||
                (
                    x.ChequeBankName != null &&
                    x.ChequeBankName.ToLower().Contains(searchTerm)
                ));
        }

        if (request.OrderId.HasValue)
        {
            query = query.Where(x =>
                x.OrderId == request.OrderId.Value);
        }

        if (request.PaymentMethodId.HasValue)
        {
            query = query.Where(x =>
                x.PaymentMethodId == request.PaymentMethodId.Value);
        }

        if (request.PaymentStatusId.HasValue)
        {
            query = query.Where(x =>
                x.PaymentStatusId == request.PaymentStatusId.Value);
        }

        if (request.FromDate.HasValue)
        {
            var fromDateUtc = request.FromDate.Value.ToDateTime(
                TimeOnly.MinValue,
                DateTimeKind.Utc);

            query = query.Where(x =>
                x.CreatedAt >= fromDateUtc);
        }

        if (request.ToDate.HasValue)
        {
            var toDateExclusiveUtc = request.ToDate.Value
                .AddDays(1)
                .ToDateTime(
                    TimeOnly.MinValue,
                    DateTimeKind.Utc);

            query = query.Where(x =>
                x.CreatedAt < toDateExclusiveUtc);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var payments = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToArrayAsync(cancellationToken);

        var responseItems = new List<PaymentResponse>();

        foreach (var payment in payments)
        {
            var summary = await PaymentStatusUpdater.GetSummaryAsync(
                _dbContext,
                payment.Order,
                cancellationToken);

            responseItems.Add(
                PaymentResponseMapper.Map(payment, summary));
        }

        var totalPages = (int)Math.Ceiling(
            totalCount / (double)request.PageSize);

        return new PagedResponse<PaymentResponse>(
            responseItems,
            totalCount,
            request.PageNumber,
            request.PageSize,
            totalPages,
            request.PageNumber > 1,
            request.PageNumber < totalPages);
    }
}