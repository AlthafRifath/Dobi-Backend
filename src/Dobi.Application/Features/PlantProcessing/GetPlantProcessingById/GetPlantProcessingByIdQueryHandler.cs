using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.PlantProcessing;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.PlantProcessing.GetPlantProcessingById
{
    public sealed class GetPlantProcessingByIdQueryHandler
    : IRequestHandler<GetPlantProcessingByIdQuery, PlantProcessingResponse>
    {
        private readonly IDobiDbContext _dbContext;

        public GetPlantProcessingByIdQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PlantProcessingResponse> Handle(
            GetPlantProcessingByIdQuery request,
            CancellationToken cancellationToken)
        {
            var processing = await _dbContext.PlantProcessings
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
                .FirstOrDefaultAsync(x => x.Id == request.PlantProcessingId, cancellationToken);

            if (processing is null)
            {
                throw new NotFoundException("Plant processing", request.PlantProcessingId);
            }

            return PlantProcessingResponseMapper.Map(processing);
        }
    }
}
