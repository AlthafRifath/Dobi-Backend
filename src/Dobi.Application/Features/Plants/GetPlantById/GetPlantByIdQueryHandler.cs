using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Plants;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Plants.GetPlantById
{
    public sealed class GetPlantByIdQueryHandler
    : IRequestHandler<GetPlantByIdQuery, PlantResponse>
    {
        private readonly IDobiDbContext _dbContext;

        public GetPlantByIdQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PlantResponse> Handle(
            GetPlantByIdQuery request,
            CancellationToken cancellationToken)
        {
            var plant = await _dbContext.Plants
                .AsNoTracking()
                .Where(x => x.Id == request.PlantId)
                .Select(x => new PlantResponse(
                    x.Id,
                    x.PlantName,
                    x.Address,
                    x.OperatingHours,
                    x.IsActive))
                .FirstOrDefaultAsync(cancellationToken);

            if (plant is null)
            {
                throw new NotFoundException("Plant", request.PlantId);
            }

            return plant;
        }
    }
}
