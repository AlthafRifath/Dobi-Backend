using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Common;
using Dobi.Contracts.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Services.GetServices
{
    public sealed class GetServicesQueryHandler
    : IRequestHandler<GetServicesQuery, PagedResponse<ServiceResponse>>
    {
        private readonly IDobiDbContext _dbContext;

        public GetServicesQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResponse<ServiceResponse>> Handle(
            GetServicesQuery request,
            CancellationToken cancellationToken)
        {
            var query = _dbContext.Services.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.Trim().ToLower();

                query = query.Where(x =>
                    x.ServiceCode.ToLower().Contains(searchTerm) ||
                    x.ServiceName.ToLower().Contains(searchTerm) ||
                    (x.Description != null && x.Description.ToLower().Contains(searchTerm)));
            }

            if (request.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == request.IsActive.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var services = await query
                .OrderBy(x => x.ServiceName)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ServiceResponse(
                    x.Id,
                    x.ServiceCode,
                    x.ServiceName,
                    x.Description,
                    x.IsExpressEligible,
                    x.IsActive))
                .ToArrayAsync(cancellationToken);

            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            return new PagedResponse<ServiceResponse>(
                services,
                totalCount,
                request.PageNumber,
                request.PageSize,
                totalPages,
                request.PageNumber > 1,
                request.PageNumber < totalPages);
        }
    }
}
