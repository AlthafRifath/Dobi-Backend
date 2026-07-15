using Dobi.Application.Abstractions.Persistence;
using Dobi.Application.Abstractions.Services;
using Dobi.Contracts.Plants;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Plants.UpdatePlantStatus
{
    public sealed class UpdatePlantStatusCommandHandler
    : IRequestHandler<UpdatePlantStatusCommand, PlantResponse>
    {
        private readonly IDobiDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public UpdatePlantStatusCommandHandler(
            IDobiDbContext dbContext,
            ICurrentUserService currentUserService,
            IDateTimeProvider dateTimeProvider)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<PlantResponse> Handle(
            UpdatePlantStatusCommand request,
            CancellationToken cancellationToken)
        {
            var plant = await _dbContext.Plants
                .FirstOrDefaultAsync(x => x.Id == request.PlantId, cancellationToken);

            if (plant is null)
            {
                throw new NotFoundException("Plant", request.PlantId);
            }

            plant.IsActive = request.IsActive;
            plant.UpdatedAt = _dateTimeProvider.UtcNow;
            plant.UpdatedByUserId = _currentUserService.UserId;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new PlantResponse(
                plant.Id,
                plant.PlantName,
                plant.Address,
                plant.OperatingHours,
                plant.IsActive);
        }
    }
}
