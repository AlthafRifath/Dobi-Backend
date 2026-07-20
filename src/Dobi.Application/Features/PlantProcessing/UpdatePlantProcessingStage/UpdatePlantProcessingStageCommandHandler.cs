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

namespace Dobi.Application.Features.PlantProcessing.UpdatePlantProcessingStage
{
    public sealed class UpdatePlantProcessingStageCommandHandler
    : IRequestHandler<UpdatePlantProcessingStageCommand, PlantProcessingResponse>
    {
        private readonly IDobiDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public UpdatePlantProcessingStageCommandHandler(
            IDobiDbContext dbContext,
            ICurrentUserService currentUserService,
            IDateTimeProvider dateTimeProvider)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<PlantProcessingResponse> Handle(
            UpdatePlantProcessingStageCommand request,
            CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId is null)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }

            var plantProcessing = await _dbContext.PlantProcessings
                .Include(x => x.Order)
                    .ThenInclude(x => x.CurrentStatus)
                .FirstOrDefaultAsync(x => x.Id == request.PlantProcessingId, cancellationToken);

            if (plantProcessing is null)
            {
                throw new NotFoundException("Plant processing", request.PlantProcessingId);
            }

            var stage = await _dbContext.ProcessingStages
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.ProcessingStageId, cancellationToken);

            if (stage is null)
            {
                throw new NotFoundException("Processing stage", request.ProcessingStageId);
            }

            if (!stage.IsActive)
            {
                throw new BadRequestException("Selected processing stage is inactive.");
            }

            var stageStatus = await _dbContext.ProcessingStageStatuses
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.ProcessingStageStatusId, cancellationToken);

            if (stageStatus is null)
            {
                throw new NotFoundException("Processing stage status", request.ProcessingStageStatusId);
            }

            var orderStatusCode = LookupValueHelper.GetCode(plantProcessing.Order.CurrentStatus);

            if (orderStatusCode is OrderStatusCodes.ReadyForOutletReturn
                or OrderStatusCodes.ReturnedToOutlet
                or OrderStatusCodes.ReadyForCollection
                or OrderStatusCodes.CollectedDelivered
                or OrderStatusCodes.Closed)
            {
                throw new ConflictException("Plant processing cannot be updated after the order has left the plant workflow.");
            }

            var now = _dateTimeProvider.UtcNow;
            var stageStatusCode = LookupValueHelper.GetCode(stageStatus);

            var stageUpdate = new PlantProcessingStageUpdate
            {
                PlantProcessingId = plantProcessing.Id,
                ProcessingStageId = request.ProcessingStageId,
                ProcessingStageStatusId = request.ProcessingStageStatusId,
                StartedAt = stageStatusCode is ProcessingStageStatusCodes.Pending
                    or ProcessingStageStatusCodes.InProgress
                    ? now
                    : null,
                CompletedAt = stageStatusCode is ProcessingStageStatusCodes.Done
                    or ProcessingStageStatusCodes.Failed
                    or ProcessingStageStatusCodes.NotRequired
                    ? now
                    : null,
                UpdatedByUserId = _currentUserService.UserId.Value,
                Remarks = string.IsNullOrWhiteSpace(request.Remarks)
                    ? null
                    : request.Remarks.Trim()
            };

            _dbContext.PlantProcessingStageUpdates.Add(stageUpdate);

            var stageCode = LookupValueHelper.GetCode(stage);

            if (stageCode == ProcessingStageCodes.Qc &&
                stageStatusCode is ProcessingStageStatusCodes.Pending or ProcessingStageStatusCodes.InProgress)
            {
                var qcPendingStatus = await GetOrderStatusByCodeAsync(
                    OrderStatusCodes.QcPending,
                    cancellationToken);

                ChangeOrderStatus(
                    plantProcessing.Order,
                    qcPendingStatus,
                    "Order moved to QC pending.");
            }

            if (stageCode == ProcessingStageCodes.Packing &&
                stageStatusCode == ProcessingStageStatusCodes.Done)
            {
                var packedStatus = await GetOrderStatusByCodeAsync(
                    OrderStatusCodes.Packed,
                    cancellationToken);

                ChangeOrderStatus(
                    plantProcessing.Order,
                    packedStatus,
                    "Order packed after plant processing.");
            }

            plantProcessing.UpdatedAt = now;
            plantProcessing.UpdatedByUserId = _currentUserService.UserId;

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

        private void ChangeOrderStatus(
            Order order,
            OrderStatus newStatus,
            string remarks)
        {
            if (order.CurrentStatusId == newStatus.Id)
            {
                return;
            }

            order.CurrentStatusId = newStatus.Id;
            order.UpdatedAt = _dateTimeProvider.UtcNow;
            order.UpdatedByUserId = _currentUserService.UserId;

            _dbContext.OrderStatusHistories.Add(new OrderStatusHistory
            {
                OrderId = order.Id,
                OrderStatusId = newStatus.Id,
                Remarks = remarks,
                ChangedByUserId = _currentUserService.UserId!.Value,
                ChangedAt = _dateTimeProvider.UtcNow
            });
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
