using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Common;
using Dobi.Contracts.PlantProcessing;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.PlantProcessing.GetPlantProcessing
{
    public sealed class GetPlantProcessingQueryHandler
    : IRequestHandler<GetPlantProcessingQuery, PagedResponse<PlantProcessingResponse>>
    {
        private readonly IDobiDbContext _dbContext;

        public GetPlantProcessingQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResponse<PlantProcessingResponse>> Handle(
            GetPlantProcessingQuery request,
            CancellationToken cancellationToken)
        {
            var query = _dbContext.PlantProcessings
                .AsNoTracking()
                .Include(x => x.Order)
                    .ThenInclude(x => x.CurrentStatus)
                .Include(x => x.Plant)
                .Include(x => x.OverallQCStatus)
                .Include(x => x.StageUpdates)
                    .ThenInclude(x => x.ProcessingStage)
                .Include(x => x.StageUpdates)
                    .ThenInclude(x => x.ProcessingStageStatus)
                .Include(x => x.QCRecords)
                    .ThenInclude(x => x.QCStatus)
                .AsQueryable();

            if (request.PlantId.HasValue)
            {
                query = query.Where(x => x.PlantId == request.PlantId.Value);
            }

            if (request.OrderId.HasValue)
            {
                query = query.Where(x => x.OrderId == request.OrderId.Value);
            }

            if (request.FromDate.HasValue)
            {
                var fromDate = request.FromDate.Value.ToDateTime(TimeOnly.MinValue);
                query = query.Where(x => x.ReceivedAtPlant >= fromDate);
            }

            if (request.ToDate.HasValue)
            {
                var toDate = request.ToDate.Value.ToDateTime(TimeOnly.MaxValue);
                query = query.Where(x => x.ReceivedAtPlant <= toDate);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(x => x.ReceivedAtPlant)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToArrayAsync(cancellationToken);

            var responseItems = items
                .Select(PlantProcessingResponseMapper.Map)
                .ToArray();

            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            return new PagedResponse<PlantProcessingResponse>(
                responseItems,
                totalCount,
                request.PageNumber,
                request.PageSize,
                totalPages,
                request.PageNumber > 1,
                request.PageNumber < totalPages);
        }
    }
}
