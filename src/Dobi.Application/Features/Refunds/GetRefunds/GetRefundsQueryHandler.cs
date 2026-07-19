using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Common;
using Dobi.Contracts.Refunds;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dobi.Application.Features.Refunds.GetRefunds;

public sealed class GetRefundsQueryHandler
    : IRequestHandler<GetRefundsQuery, PagedResponse<RefundResponse>>
{
    private readonly IDobiDbContext _dbContext;

    public GetRefundsQueryHandler(IDobiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResponse<RefundResponse>> Handle(
        GetRefundsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.Refunds
            .AsNoTracking()
            .Include(x => x.Order)
            .Include(x => x.RefundStatus)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm
                .Trim()
                .ToLower();

            query = query.Where(x =>
                x.Order.OrderNo.ToLower().Contains(searchTerm) ||
                x.Reason.ToLower().Contains(searchTerm) ||
                (
                    x.RejectionReason != null &&
                    x.RejectionReason.ToLower().Contains(searchTerm)
                ));
        }

        if (request.OrderId.HasValue)
        {
            query = query.Where(x =>
                x.OrderId == request.OrderId.Value);
        }

        if (request.PaymentId.HasValue)
        {
            query = query.Where(x =>
                x.PaymentId == request.PaymentId.Value);
        }

        if (request.RefundStatusId.HasValue)
        {
            query = query.Where(x =>
                x.RefundStatusId == request.RefundStatusId.Value);
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

        var refunds = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToArrayAsync(cancellationToken);

        var responseItems = refunds
            .Select(RefundResponseMapper.Map)
            .ToArray();

        var totalPages = (int)Math.Ceiling(
            totalCount / (double)request.PageSize);

        return new PagedResponse<RefundResponse>(
            responseItems,
            totalCount,
            request.PageNumber,
            request.PageSize,
            totalPages,
            request.PageNumber > 1,
            request.PageNumber < totalPages);
    }
}