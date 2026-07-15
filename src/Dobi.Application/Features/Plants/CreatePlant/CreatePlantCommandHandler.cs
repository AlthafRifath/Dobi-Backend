using Dobi.Application.Abstractions.Persistence;
using Dobi.Application.Abstractions.Services;
using Dobi.Contracts.Plants;
using Dobi.Domain.Plants;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Plants.CreatePlant
{
    public sealed class CreatePlantCommandHandler
    : IRequestHandler<CreatePlantCommand, PlantResponse>
    {
        private readonly IDobiDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CreatePlantCommandHandler(
            IDobiDbContext dbContext,
            ICurrentUserService currentUserService,
            IDateTimeProvider dateTimeProvider)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<PlantResponse> Handle(
            CreatePlantCommand request,
            CancellationToken cancellationToken)
        {
            var plantName = request.PlantName.Trim();

            var exists = await _dbContext.Plants.AnyAsync(
                x => x.PlantName.ToLower() == plantName.ToLower(),
                cancellationToken);

            if (exists)
            {
                throw new ConflictException("A plant with the same name already exists.");
            }

            var plant = new Plant
            {
                PlantName = plantName,
                Address = string.IsNullOrWhiteSpace(request.Address) ? null : request.Address.Trim(),
                OperatingHours = string.IsNullOrWhiteSpace(request.OperatingHours) ? null : request.OperatingHours.Trim(),
                IsActive = request.IsActive,
                CreatedAt = _dateTimeProvider.UtcNow,
                CreatedByUserId = _currentUserService.UserId
            };

            _dbContext.Plants.Add(plant);

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
