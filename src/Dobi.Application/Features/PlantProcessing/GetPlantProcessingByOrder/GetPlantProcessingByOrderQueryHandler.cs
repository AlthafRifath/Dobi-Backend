using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.PlantProcessing;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.PlantProcessing.GetPlantProcessingByOrder
{
    public sealed class GetPlantProcessingByOrderQueryHandler
    : IRequestHandler<GetPlantProcessingByOrderQuery, PlantProcessingResponse>
    {
        private readonly IDobiDbContext _dbContext;

        public GetPlantProcessingByOrderQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PlantProcessingResponse> Handle(
            GetPlantProcessingByOrderQuery request,
            CancellationToken cancellationToken)
        {
            var processing = await _dbContext.PlantProcessings
                .AsNoTracking()
                .Include(x => x.Order)
                    .ThenInclude(x => x.CurrentStatus)
                .Include(x => x.Order)
                    .ThenInclude(x => x.Items)
                        .ThenInclude(x => x.Service)
                .Include(x => x.Order)
                    .ThenInclude(x => x.Items)
                        .ThenInclude(x => x.ItemCategory)
                .Include(x => x.Order)
                    .ThenInclude(x => x.Items)
                        .ThenInclude(x => x.PricingType)
                .Include(x => x.Order)
                    .ThenInclude(x => x.Items)
                        .ThenInclude(x => x.Tags)
                .Include(x => x.Plant)
                .Include(x => x.OverallQCStatus)
                .Include(x => x.StageUpdates)
                    .ThenInclude(x => x.ProcessingStage)
                .Include(x => x.StageUpdates)
                    .ThenInclude(x => x.ProcessingStageStatus)
                .Include(x => x.QCRecords)
                    .ThenInclude(x => x.QCStatus)
                .FirstOrDefaultAsync(
                    x => x.OrderId == request.OrderId,
                    cancellationToken);

            if (processing is null)
            {
                throw new NotFoundException("Plant processing for order", request.OrderId);
            }

            return PlantProcessingResponseMapper.Map(processing);
        }
    }
}
