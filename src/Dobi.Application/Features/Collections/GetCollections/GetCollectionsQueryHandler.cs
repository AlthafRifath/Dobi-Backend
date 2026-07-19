using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Collections;
using Dobi.Contracts.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dobi.Application.Features.Collections.GetCollections;

public sealed class GetCollectionsQueryHandler
    : IRequestHandler<
        GetCollectionsQuery,
        PagedResponse<OrderCollectionResponse>>
{
    private readonly IDobiDbContext _dbContext;

    public GetCollectionsQueryHandler(IDobiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResponse<OrderCollectionResponse>> Handle(
        GetCollectionsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.OrderCollections
            .AsNoTracking()
            .Include(x => x.CollectionMode)
            .Include(x => x.Order)
                .ThenInclude(x => x.Customer)
                    .ThenInclude(x => x.CustomerType)
            .Include(x => x.Order)
                .ThenInclude(x => x.Branch)
            .Include(x => x.Order)
                .ThenInclude(x => x.CurrentStatus)
            .Include(x => x.Order)
                .ThenInclude(x => x.PaymentStatus)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm
                .Trim()
                .ToLower();

            query = query.Where(x =>
                x.Order.OrderNo.ToLower().Contains(searchTerm) ||
                x.Order.Customer.FullName.ToLower().Contains(searchTerm) ||
                x.Order.Customer.MobileNo.ToLower().Contains(searchTerm) ||
                (
                    x.CollectorName != null &&
                    x.CollectorName.ToLower().Contains(searchTerm)
                ) ||
                (
                    x.CollectorMobileNo != null &&
                    x.CollectorMobileNo.ToLower().Contains(searchTerm)
                ));
        }

        if (request.CollectionModeId.HasValue)
        {
            query = query.Where(x =>
                x.CollectionModeId == request.CollectionModeId.Value);
        }

        if (request.FromDate.HasValue)
        {
            var fromDateUtc = request.FromDate.Value.ToDateTime(
                TimeOnly.MinValue,
                DateTimeKind.Utc);

            query = query.Where(x =>
                x.CollectedOrDeliveredAt >= fromDateUtc);
        }

        if (request.ToDate.HasValue)
        {
            var toDateExclusiveUtc = request.ToDate.Value
                .AddDays(1)
                .ToDateTime(
                    TimeOnly.MinValue,
                    DateTimeKind.Utc);

            query = query.Where(x =>
                x.CollectedOrDeliveredAt < toDateExclusiveUtc);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var collections = await query
            .OrderByDescending(x => x.CollectedOrDeliveredAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToArrayAsync(cancellationToken);

        var responseItems = collections
            .Select(OrderCollectionResponseMapper.Map)
            .ToArray();

        var totalPages = (int)Math.Ceiling(
            totalCount / (double)request.PageSize);

        return new PagedResponse<OrderCollectionResponse>(
            responseItems,
            totalCount,
            request.PageNumber,
            request.PageSize,
            totalPages,
            request.PageNumber > 1,
            request.PageNumber < totalPages);
    }
}