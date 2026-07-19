using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Common;
using Dobi.Contracts.Transfers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dobi.Application.Features.Transfers.GetTransfers;

public sealed class GetTransfersQueryHandler
    : IRequestHandler<GetTransfersQuery, PagedResponse<TransferBatchResponse>>
{
    private readonly IDobiDbContext _dbContext;

    public GetTransfersQueryHandler(IDobiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResponse<TransferBatchResponse>> Handle(
        GetTransfersQuery request,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.TransferBatches
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm
                .Trim()
                .ToLower();

            query = query.Where(x =>
                x.TransferNo.ToLower().Contains(searchTerm) ||
                (x.Remarks != null &&
                 x.Remarks.ToLower().Contains(searchTerm)));
        }

        if (request.TransferTypeId.HasValue)
        {
            query = query.Where(x =>
                x.TransferTypeId == request.TransferTypeId.Value);
        }

        if (request.TransferStatusId.HasValue)
        {
            query = query.Where(x =>
                x.TransferStatusId == request.TransferStatusId.Value);
        }

        if (request.DriverUserId.HasValue)
        {
            query = query.Where(x =>
                x.DriverUserId == request.DriverUserId.Value);
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

        var transfers = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToArrayAsync(cancellationToken);

        var responseItems = new List<TransferBatchResponse>();

        foreach (var transfer in transfers)
        {
            var response = await TransferResponseMapper.MapAsync(
                _dbContext,
                transfer,
                cancellationToken);

            responseItems.Add(response);
        }

        var totalPages = (int)Math.Ceiling(
            totalCount / (double)request.PageSize);

        return new PagedResponse<TransferBatchResponse>(
            responseItems,
            totalCount,
            request.PageNumber,
            request.PageSize,
            totalPages,
            request.PageNumber > 1,
            request.PageNumber < totalPages);
    }
}