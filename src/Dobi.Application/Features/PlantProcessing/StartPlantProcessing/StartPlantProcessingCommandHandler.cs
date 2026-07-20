using Dobi.Application.Abstractions.Persistence;
using Dobi.Application.Abstractions.Services;
using Dobi.Application.Common;
using Dobi.Contracts.PlantProcessing;
using Dobi.Domain.Orders;
using Dobi.Domain.PlantProcessing;
using Dobi.Shared.Constants;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PlantProcessingEntity = Dobi.Domain.PlantProcessing.PlantProcessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.PlantProcessing.StartPlantProcessing
{
    public sealed class StartPlantProcessingCommandHandler
    : IRequestHandler<StartPlantProcessingCommand, PlantProcessingResponse>
    {
        private readonly IDobiDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public StartPlantProcessingCommandHandler(
            IDobiDbContext dbContext,
            ICurrentUserService currentUserService,
            IDateTimeProvider dateTimeProvider)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<PlantProcessingResponse> Handle(
            StartPlantProcessingCommand request,
            CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId is null)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }

            var order = await _dbContext.Orders
                .Include(x => x.CurrentStatus)
                .FirstOrDefaultAsync(x => x.Id == request.OrderId, cancellationToken);

            if (order is null)
            {
                throw new NotFoundException("Order", request.OrderId);
            }

            var currentOrderStatusCode = LookupValueHelper.GetCode(order.CurrentStatus);

            if (currentOrderStatusCode != OrderStatusCodes.ReceivedAtPlant)
            {
                throw new ConflictException("Only orders received at plant can start plant processing.");
            }

            var plant = await _dbContext.Plants
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.PlantId, cancellationToken);

            if (plant is null)
            {
                throw new NotFoundException("Plant", request.PlantId);
            }

            if (!plant.IsActive)
            {
                throw new BadRequestException("Selected plant is inactive.");
            }

            var alreadyExists = await _dbContext.PlantProcessings.AnyAsync(
                x => x.OrderId == request.OrderId,
                cancellationToken);

            if (alreadyExists)
            {
                throw new ConflictException("Plant processing has already been started for this order.");
            }

            var processingOrderStatus = await GetOrderStatusByCodeAsync(
                OrderStatusCodes.Processing,
                cancellationToken);

            var washingStage = await GetProcessingStageByCodeAsync(
                ProcessingStageCodes.Washing,
                cancellationToken);

            var inProgressStageStatus = await GetProcessingStageStatusByCodeAsync(
                ProcessingStageStatusCodes.InProgress,
                cancellationToken);

            var now = _dateTimeProvider.UtcNow;

            var plantProcessing = new PlantProcessingEntity
            {
                OrderId = request.OrderId,
                PlantId = request.PlantId,
                ReceivedAtPlant = now,
                ReadyDate = null,
                OverallQCStatusId = null,
                PlantRemarks = string.IsNullOrWhiteSpace(request.PlantRemarks)
                    ? null
                    : request.PlantRemarks.Trim(),
                CreatedAt = now,
                CreatedByUserId = _currentUserService.UserId
            };

            _dbContext.PlantProcessings.Add(plantProcessing);

            order.CurrentStatusId = processingOrderStatus.Id;
            order.UpdatedAt = now;
            order.UpdatedByUserId = _currentUserService.UserId;

            _dbContext.OrderStatusHistories.Add(new OrderStatusHistory
            {
                OrderId = order.Id,
                OrderStatusId = processingOrderStatus.Id,
                Remarks = "Plant processing started.",
                ChangedByUserId = _currentUserService.UserId.Value,
                ChangedAt = now
            });

            await _dbContext.SaveChangesAsync(cancellationToken);

            _dbContext.PlantProcessingStageUpdates.Add(new PlantProcessingStageUpdate
            {
                PlantProcessingId = plantProcessing.Id,
                ProcessingStageId = washingStage.Id,
                ProcessingStageStatusId = inProgressStageStatus.Id,
                StartedAt = now,
                CompletedAt = null,
                UpdatedByUserId = _currentUserService.UserId.Value,
                Remarks = "Washing started."
            });

            await _dbContext.SaveChangesAsync(cancellationToken);

            var saved = await LoadPlantProcessingAsync(plantProcessing.Id, cancellationToken);

            return PlantProcessingResponseMapper.Map(saved);
        }

        private async Task<OrderStatus> GetOrderStatusByCodeAsync(
            string statusCode,
            CancellationToken cancellationToken)
        {
            var statuses = await _dbContext.OrderStatuses.ToListAsync(cancellationToken);

            var status = statuses.FirstOrDefault(x =>
                LookupValueHelper.GetCode(x) == statusCode);

            if (status is null)
            {
                throw new InvalidOperationException($"Order status '{statusCode}' was not found.");
            }

            return status;
        }

        private async Task<ProcessingStage> GetProcessingStageByCodeAsync(
            string stageCode,
            CancellationToken cancellationToken)
        {
            var stages = await _dbContext.ProcessingStages.ToListAsync(cancellationToken);

            var stage = stages.FirstOrDefault(x =>
                LookupValueHelper.GetCode(x) == stageCode);

            if (stage is null)
            {
                throw new InvalidOperationException($"Processing stage '{stageCode}' was not found.");
            }

            return stage;
        }

        private async Task<ProcessingStageStatus> GetProcessingStageStatusByCodeAsync(
            string statusCode,
            CancellationToken cancellationToken)
        {
            var statuses = await _dbContext.ProcessingStageStatuses.ToListAsync(cancellationToken);

            var status = statuses.FirstOrDefault(x =>
                LookupValueHelper.GetCode(x) == statusCode);

            if (status is null)
            {
                throw new InvalidOperationException($"Processing stage status '{statusCode}' was not found.");
            }

            return status;
        }

        private async Task<PlantProcessingEntity> LoadPlantProcessingAsync(
            int plantProcessingId,
            CancellationToken cancellationToken)
        {
            return await _dbContext.PlantProcessings
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
                .FirstAsync(x => x.Id == plantProcessingId, cancellationToken);
        }
    }
}
