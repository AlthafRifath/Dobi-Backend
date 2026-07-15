using Dobi.Application.Abstractions.Persistence;
using Dobi.Application.Abstractions.Services;
using Dobi.Application.Common;
using Dobi.Contracts.Transfers;
using Dobi.Domain.Orders;
using Dobi.Domain.Transfers;
using Dobi.Shared.Constants;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dobi.Application.Features.Transfers.AcknowledgeTransfer
{
    public sealed class AcknowledgeTransferCommandHandler
    : IRequestHandler<AcknowledgeTransferCommand, TransferBatchResponse>
    {
        private readonly IDobiDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public AcknowledgeTransferCommandHandler(
            IDobiDbContext dbContext,
            ICurrentUserService currentUserService,
            IDateTimeProvider dateTimeProvider)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<TransferBatchResponse> Handle(
            AcknowledgeTransferCommand request,
            CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId is null)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }

            var transferBatch = await _dbContext.TransferBatches
                .FirstOrDefaultAsync(x => x.Id == request.TransferBatchId, cancellationToken);

            if (transferBatch is null)
            {
                throw new NotFoundException("Transfer batch", request.TransferBatchId);
            }

            var acknowledgementType = await _dbContext.AcknowledgementTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.AcknowledgementTypeId, cancellationToken);

            if (acknowledgementType is null)
            {
                throw new NotFoundException("Acknowledgement type", request.AcknowledgementTypeId);
            }

            var acknowledgementCode = LookupValueHelper.GetCode(acknowledgementType);

            var duplicateAcknowledgementExists = await _dbContext.TransferAcknowledgements.AnyAsync(
                x => x.TransferBatchId == transferBatch.Id &&
                     x.AcknowledgementTypeId == request.AcknowledgementTypeId,
                cancellationToken);

            if (duplicateAcknowledgementExists)
            {
                throw new ConflictException($"Acknowledgement '{LookupValueHelper.GetName(acknowledgementType)}' has already been recorded for this transfer.");
            }

            var transferType = await _dbContext.TransferTypes
                .AsNoTracking()
                .FirstAsync(x => x.Id == transferBatch.TransferTypeId, cancellationToken);

            var transferTypeCode = LookupValueHelper.GetCode(transferType);

            await ValidateAcknowledgementForTransferTypeAsync(
                transferTypeCode,
                acknowledgementCode,
                cancellationToken);

            _dbContext.TransferAcknowledgements.Add(new TransferAcknowledgement
            {
                TransferBatchId = transferBatch.Id,
                AcknowledgementTypeId = request.AcknowledgementTypeId,
                UserId = _currentUserService.UserId.Value,
                AcknowledgedAt = _dateTimeProvider.UtcNow,
                SignatureUrl = string.IsNullOrWhiteSpace(request.SignatureUrl)
                    ? null
                    : request.SignatureUrl.Trim(),
                Remarks = string.IsNullOrWhiteSpace(request.Remarks)
                    ? null
                    : request.Remarks.Trim()
            });

            await ApplyWorkflowChangesAsync(
                transferBatch,
                transferTypeCode,
                acknowledgementCode,
                cancellationToken);

            transferBatch.UpdatedAt = _dateTimeProvider.UtcNow;
            transferBatch.UpdatedByUserId = _currentUserService.UserId;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return await TransferResponseMapper.MapAsync(
                _dbContext,
                transferBatch,
                cancellationToken);
        }

        private static Task ValidateAcknowledgementForTransferTypeAsync(
            string transferTypeCode,
            string acknowledgementCode,
            CancellationToken cancellationToken)
        {
            if (transferTypeCode == TransferTypeCodes.OutletToPlant)
            {
                var allowed = new[]
                {
                AcknowledgementTypeCodes.OutletHandover,
                AcknowledgementTypeCodes.DriverPickup,
                AcknowledgementTypeCodes.PlantReceive
            };

                if (!allowed.Contains(acknowledgementCode))
                {
                    throw new BadRequestException($"Acknowledgement '{acknowledgementCode}' is not valid for outlet-to-plant transfer.");
                }
            }
            else if (transferTypeCode == TransferTypeCodes.PlantToOutlet)
            {
                var allowed = new[]
                {
                AcknowledgementTypeCodes.PlantHandover,
                AcknowledgementTypeCodes.DriverPickup,
                AcknowledgementTypeCodes.DriverDropoff,
                AcknowledgementTypeCodes.OutletReceive
            };

                if (!allowed.Contains(acknowledgementCode))
                {
                    throw new BadRequestException($"Acknowledgement '{acknowledgementCode}' is not valid for plant-to-outlet transfer.");
                }
            }
            else
            {
                throw new BadRequestException($"Unsupported transfer type '{transferTypeCode}'.");
            }

            return Task.CompletedTask;
        }

        private async Task ApplyWorkflowChangesAsync(
            TransferBatch transferBatch,
            string transferTypeCode,
            string acknowledgementCode,
            CancellationToken cancellationToken)
        {
            if (acknowledgementCode is AcknowledgementTypeCodes.OutletHandover
                or AcknowledgementTypeCodes.DriverPickup
                or AcknowledgementTypeCodes.PlantHandover
                or AcknowledgementTypeCodes.DriverDropoff)
            {
                var inTransitStatus = await GetTransferStatusByCodeAsync(
                    TransferStatusCodes.InTransit,
                    cancellationToken);

                transferBatch.TransferStatusId = inTransitStatus.Id;
                transferBatch.SentAt ??= _dateTimeProvider.UtcNow;

                return;
            }

            if (transferTypeCode == TransferTypeCodes.OutletToPlant &&
                acknowledgementCode == AcknowledgementTypeCodes.PlantReceive)
            {
                var receivedTransferStatus = await GetTransferStatusByCodeAsync(
                    TransferStatusCodes.Received,
                    cancellationToken);

                var receivedAtPlantOrderStatus = await GetOrderStatusByCodeAsync(
                    OrderStatusCodes.ReceivedAtPlant,
                    cancellationToken);

                transferBatch.TransferStatusId = receivedTransferStatus.Id;
                transferBatch.ReceivedAt = _dateTimeProvider.UtcNow;

                await ChangeTransferOrdersStatusAsync(
                    transferBatch.Id,
                    receivedAtPlantOrderStatus,
                    $"Order received at plant from transfer {transferBatch.TransferNo}.",
                    cancellationToken);

                return;
            }

            if (transferTypeCode == TransferTypeCodes.PlantToOutlet &&
                acknowledgementCode == AcknowledgementTypeCodes.OutletReceive)
            {
                var receivedTransferStatus = await GetTransferStatusByCodeAsync(
                    TransferStatusCodes.Received,
                    cancellationToken);

                var returnedToOutletOrderStatus = await GetOrderStatusByCodeAsync(
                    OrderStatusCodes.ReturnedToOutlet,
                    cancellationToken);

                transferBatch.TransferStatusId = receivedTransferStatus.Id;
                transferBatch.ReceivedAt = _dateTimeProvider.UtcNow;

                await ChangeTransferOrdersStatusAsync(
                    transferBatch.Id,
                    returnedToOutletOrderStatus,
                    $"Order returned to outlet from transfer {transferBatch.TransferNo}.",
                    cancellationToken);
            }
        }

        private async Task ChangeTransferOrdersStatusAsync(
            int transferBatchId,
            OrderStatus newStatus,
            string remarks,
            CancellationToken cancellationToken)
        {
            var orderIds = await _dbContext.TransferBatchItems
                .AsNoTracking()
                .Where(x => x.TransferBatchId == transferBatchId)
                .Select(x => x.OrderId)
                .ToArrayAsync(cancellationToken);

            var orders = await _dbContext.Orders
                .Where(x => orderIds.Contains(x.Id))
                .ToArrayAsync(cancellationToken);

            foreach (var order in orders)
            {
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
        }

        private async Task<TransferStatus> GetTransferStatusByCodeAsync(
            string statusCode,
            CancellationToken cancellationToken)
        {
            var statuses = await _dbContext.TransferStatuses
                .ToListAsync(cancellationToken);

            var status = statuses.FirstOrDefault(x =>
                LookupValueHelper.GetCode(x) == statusCode);

            if (status is null)
            {
                throw new InvalidOperationException($"Transfer status '{statusCode}' was not found.");
            }

            return status;
        }

        private async Task<OrderStatus> GetOrderStatusByCodeAsync(
            string statusCode,
            CancellationToken cancellationToken)
        {
            var statuses = await _dbContext.OrderStatuses
                .ToListAsync(cancellationToken);

            var status = statuses.FirstOrDefault(x =>
                LookupValueHelper.GetCode(x) == statusCode);

            if (status is null)
            {
                throw new InvalidOperationException($"Order status '{statusCode}' was not found.");
            }

            return status;
        }
    }
}
