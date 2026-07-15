using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Common;
using Dobi.Contracts.Plants;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Plants.GetPlants
{
    public sealed class GetPlantsQueryHandler
    : IRequestHandler<GetPlantsQuery, PagedResponse<PlantResponse>>
    {
        private readonly IDobiDbContext _dbContext;

        public GetPlantsQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResponse<PlantResponse>> Handle(
            GetPlantsQuery request,
            CancellationToken cancellationToken)
        {
            var query = _dbContext.Plants.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.Trim().ToLower();

                query = query.Where(x =>
                    x.PlantName.ToLower().Contains(searchTerm) ||
                    (x.Address != null && x.Address.ToLower().Contains(searchTerm)) ||
                    (x.OperatingHours != null && x.OperatingHours.ToLower().Contains(searchTerm)));
            }

            if (request.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == request.IsActive.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var plants = await query
                .OrderBy(x => x.PlantName)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new PlantResponse(
                    x.Id,
                    x.PlantName,
                    x.Address,
                    x.OperatingHours,
                    x.IsActive))
                .ToArrayAsync(cancellationToken);

            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            return new PagedResponse<PlantResponse>(
                plants,
                totalCount,
                request.PageNumber,
                request.PageSize,
                totalPages,
                request.PageNumber > 1,
                request.PageNumber < totalPages);
        }
    }
}
