using Dobi.Application.Abstractions.Persistence;
using Dobi.Application.Abstractions.Services;
using Dobi.Contracts.Plants;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Plants.UpdatePlant
{
    public sealed class UpdatePlantCommandHandler
    : IRequestHandler<UpdatePlantCommand, PlantResponse>
    {
        private readonly IDobiDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public UpdatePlantCommandHandler(
            IDobiDbContext dbContext,
            ICurrentUserService currentUserService,
            IDateTimeProvider dateTimeProvider)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<PlantResponse> Handle(
            UpdatePlantCommand request,
            CancellationToken cancellationToken)
        {
            var plant = await _dbContext.Plants
                .FirstOrDefaultAsync(x => x.Id == request.PlantId, cancellationToken);

            if (plant is null)
            {
                throw new NotFoundException("Plant", request.PlantId);
            }

            var plantName = request.PlantName.Trim();

            var duplicateExists = await _dbContext.Plants.AnyAsync(
                x => x.Id != request.PlantId &&
                     x.PlantName.ToLower() == plantName.ToLower(),
                cancellationToken);

            if (duplicateExists)
            {
                throw new ConflictException("A plant with the same name already exists.");
            }

            plant.PlantName = plantName;
            plant.Address = string.IsNullOrWhiteSpace(request.Address) ? null : request.Address.Trim();
            plant.OperatingHours = string.IsNullOrWhiteSpace(request.OperatingHours) ? null : request.OperatingHours.Trim();
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
