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

namespace Dobi.Application.Features.PlantProcessing.RecordQc
{
    public sealed class RecordQcCommandHandler
    : IRequestHandler<RecordQcCommand, PlantProcessingResponse>
    {
        private readonly IDobiDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public RecordQcCommandHandler(
            IDobiDbContext dbContext,
            ICurrentUserService currentUserService,
            IDateTimeProvider dateTimeProvider)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<PlantProcessingResponse> Handle(
            RecordQcCommand request,
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

            if (request.OrderItemId.HasValue)
            {
                var itemBelongsToOrder = await _dbContext.OrderItems.AnyAsync(
                    x => x.Id == request.OrderItemId.Value &&
                         x.OrderId == plantProcessing.OrderId,
                    cancellationToken);

                if (!itemBelongsToOrder)
                {
                    throw new BadRequestException("Selected order item does not belong to this order.");
                }
            }

            var qcStatus = await _dbContext.QCStatuses
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.QcStatusId, cancellationToken);

            if (qcStatus is null)
            {
                throw new NotFoundException("QC status", request.QcStatusId);
            }

            var now = _dateTimeProvider.UtcNow;

            _dbContext.QCRecords.Add(new QCRecord
            {
                PlantProcessingId = plantProcessing.Id,
                OrderItemId = request.OrderItemId,
                QCStatusId = request.QcStatusId,
                IssueDescription = string.IsNullOrWhiteSpace(request.IssueDescription)
                    ? null
                    : request.IssueDescription.Trim(),
                ActionTaken = string.IsNullOrWhiteSpace(request.ActionTaken)
                    ? null
                    : request.ActionTaken.Trim(),
                LabourChargeAmount = request.LabourChargeAmount,
                RecordedByUserId = _currentUserService.UserId.Value,
                RecordedAt = now
            });

            plantProcessing.OverallQCStatusId = request.QcStatusId;

            var qcStage = await GetProcessingStageByCodeAsync(
                ProcessingStageCodes.Qc,
                cancellationToken);

            var qcStatusCode = LookupValueHelper.GetCode(qcStatus);

            if (qcStatusCode == QCStatusCodes.Failed)
            {
                var failedStageStatus = await GetProcessingStageStatusByCodeAsync(
                    ProcessingStageStatusCodes.Failed,
                    cancellationToken);

                _dbContext.PlantProcessingStageUpdates.Add(new PlantProcessingStageUpdate
                {
                    PlantProcessingId = plantProcessing.Id,
                    ProcessingStageId = qcStage.Id,
                    ProcessingStageStatusId = failedStageStatus.Id,
                    StartedAt = null,
                    CompletedAt = now,
                    UpdatedByUserId = _currentUserService.UserId.Value,
                    Remarks = request.IssueDescription
                });

                var qcFailedOrderStatus = await GetOrderStatusByCodeAsync(
                    OrderStatusCodes.QcFailed,
                    cancellationToken);

                ChangeOrderStatus(
                    plantProcessing.Order,
                    qcFailedOrderStatus,
                    "QC failed. Action required.");
            }
            else if (qcStatusCode == QCStatusCodes.Passed)
            {
                var doneStageStatus = await GetProcessingStageStatusByCodeAsync(
                    ProcessingStageStatusCodes.Done,
                    cancellationToken);

                _dbContext.PlantProcessingStageUpdates.Add(new PlantProcessingStageUpdate
                {
                    PlantProcessingId = plantProcessing.Id,
                    ProcessingStageId = qcStage.Id,
                    ProcessingStageStatusId = doneStageStatus.Id,
                    StartedAt = null,
                    CompletedAt = now,
                    UpdatedByUserId = _currentUserService.UserId.Value,
                    Remarks = "QC passed."
                });
            }
            else
            {
                throw new BadRequestException($"Unsupported QC status '{qcStatusCode}'.");
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
