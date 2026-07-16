using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.AuditLogs;
using Dobi.Contracts.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.AuditLogs.GetAuditLogs
{
    public sealed class GetAuditLogsQueryHandler
    : IRequestHandler<GetAuditLogsQuery, PagedResponse<AuditLogResponse>>
    {
        private readonly IDobiDbContext _dbContext;

        public GetAuditLogsQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResponse<AuditLogResponse>> Handle(
            GetAuditLogsQuery request,
            CancellationToken cancellationToken)
        {
            var query = _dbContext.AuditLogs
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.Trim().ToLower();

                query = query.Where(x =>
                    x.EntityName.ToLower().Contains(searchTerm) ||
                    x.Action.ToLower().Contains(searchTerm) ||
                    (x.OldValues != null && x.OldValues.ToLower().Contains(searchTerm)) ||
                    (x.NewValues != null && x.NewValues.ToLower().Contains(searchTerm)) ||
                    (x.IpAddress != null && x.IpAddress.ToLower().Contains(searchTerm)));
            }

            if (!string.IsNullOrWhiteSpace(request.EntityName))
            {
                var entityName = request.EntityName.Trim().ToLower();
                query = query.Where(x => x.EntityName.ToLower() == entityName);
            }

            if (request.EntityId.HasValue)
            {
                query = query.Where(x => x.EntityId == request.EntityId.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.Action))
            {
                var action = request.Action.Trim().ToLower();
                query = query.Where(x => x.Action.ToLower() == action);
            }

            if (request.PerformedByUserId.HasValue)
            {
                query = query.Where(x => x.PerformedByUserId == request.PerformedByUserId.Value);
            }

            if (request.FromDate.HasValue)
            {
                var fromDate = request.FromDate.Value.ToDateTime(TimeOnly.MinValue);
                query = query.Where(x => x.PerformedAt >= fromDate);
            }

            if (request.ToDate.HasValue)
            {
                var toDate = request.ToDate.Value.ToDateTime(TimeOnly.MaxValue);
                query = query.Where(x => x.PerformedAt <= toDate);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var logs = await query
                .OrderByDescending(x => x.PerformedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToArrayAsync(cancellationToken);

            var items = logs
                .Select(AuditLogResponseMapper.Map)
                .ToArray();

            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            return new PagedResponse<AuditLogResponse>(
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
