using Dobi.Application.Abstractions.Authentication;
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
using System.Text;

namespace Dobi.Application.Features.Transfers.CreateTransferBatch
{
    public sealed class CreateTransferBatchCommandHandler
    : IRequestHandler<CreateTransferBatchCommand, TransferBatchResponse>
    {
        private readonly IDobiDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CreateTransferBatchCommandHandler(
            IDobiDbContext dbContext,
            IIdentityService identityService,
            ICurrentUserService currentUserService,
            IDateTimeProvider dateTimeProvider)
        {
            _dbContext = dbContext;
            _identityService = identityService;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<TransferBatchResponse> Handle(
            CreateTransferBatchCommand request,
            CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId is null)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }

            var transferType = await _dbContext.TransferTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.TransferTypeId, cancellationToken);

            if (transferType is null)
            {
                throw new NotFoundException("Transfer type", request.TransferTypeId);
            }

            var transferTypeCode = LookupValueHelper.GetCode(transferType);

            ValidateTransferLocations(
                transferTypeCode,
                request.FromBranchId,
                request.FromPlantId,
                request.ToBranchId,
                request.ToPlantId);

            await ValidateLocationsAsync(request, transferTypeCode, cancellationToken);
            await ValidateDriverAsync(request.DriverUserId, cancellationToken);

            var createdTransferStatus = await GetTransferStatusByCodeAsync(
                TransferStatusCodes.Created,
                cancellationToken);

            var receivedTransferStatus = await GetTransferStatusByCodeAsync(
                TransferStatusCodes.Received,
                cancellationToken);

            var orderIds = request.Items
                .Select(x => x.OrderId)
                .Distinct()
                .ToArray();

            if (orderIds.Length != request.Items.Count)
            {
                throw new BadRequestException("Duplicate orders are not allowed inside the same transfer batch.");
            }

            var activeTransferOrderId = await (
                from transferItem in _dbContext.TransferBatchItems
                join existingTransferBatch in _dbContext.TransferBatches
                    on transferItem.TransferBatchId equals existingTransferBatch.Id
                where orderIds.Contains(transferItem.OrderId)
                      && existingTransferBatch.TransferStatusId != receivedTransferStatus.Id
                select transferItem.OrderId)
                .FirstOrDefaultAsync(cancellationToken);

            if (activeTransferOrderId > 0)
            {
                throw new ConflictException($"Order '{activeTransferOrderId}' is already assigned to an active transfer.");
            }

            var orders = await _dbContext.Orders
                .Include(x => x.CurrentStatus)
                .Where(x => orderIds.Contains(x.Id))
                .ToListAsync(cancellationToken);

            if (orders.Count != orderIds.Length)
            {
                throw new NotFoundException("One or more selected orders were not found.");
            }

            await ValidateOrdersForTransferAsync(
                orders,
                transferTypeCode,
                request.FromBranchId,
                request.FromPlantId,
                cancellationToken);

            var transferBatch = new TransferBatch
            {
                TransferNo = $"TMP-{Guid.NewGuid():N}"[..30],
                TransferTypeId = request.TransferTypeId,
                TransferStatusId = createdTransferStatus.Id,
                FromBranchId = request.FromBranchId,
                FromPlantId = request.FromPlantId,
                ToBranchId = request.ToBranchId,
                ToPlantId = request.ToPlantId,
                DriverUserId = request.DriverUserId,
                SentAt = null,
                ReceivedAt = null,
                Remarks = string.IsNullOrWhiteSpace(request.Remarks)
                    ? null
                    : request.Remarks.Trim(),
                CreatedAt = _dateTimeProvider.UtcNow,
                CreatedByUserId = _currentUserService.UserId
            };

            _dbContext.TransferBatches.Add(transferBatch);

            await _dbContext.SaveChangesAsync(cancellationToken);

            transferBatch.TransferNo = $"TRF-{transferBatch.Id:D6}";

            foreach (var item in request.Items)
            {
                _dbContext.TransferBatchItems.Add(new TransferBatchItem
                {
                    TransferBatchId = transferBatch.Id,
                    OrderId = item.OrderId,
                    NoOfBags = item.NoOfBags,
                    NoOfPieces = item.NoOfPieces,
                    Remarks = string.IsNullOrWhiteSpace(item.Remarks)
                        ? null
                        : item.Remarks.Trim()
                });
            }

            if (transferTypeCode == TransferTypeCodes.OutletToPlant)
            {
                var sentToPlantStatus = await GetOrderStatusByCodeAsync(
                    OrderStatusCodes.SentToPlant,
                    cancellationToken);

                foreach (var order in orders)
                {
                    ChangeOrderStatus(
                        order,
                        sentToPlantStatus,
                        $"Order assigned to outlet-to-plant transfer {transferBatch.TransferNo}.");
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return await TransferResponseMapper.MapAsync(
                _dbContext,
                transferBatch,
                cancellationToken);
        }

        private static void ValidateTransferLocations(
            string transferTypeCode,
            int? fromBranchId,
            int? fromPlantId,
            int? toBranchId,
            int? toPlantId)
        {
            if (transferTypeCode == TransferTypeCodes.OutletToPlant)
            {
                if (!fromBranchId.HasValue || !toPlantId.HasValue)
                {
                    throw new BadRequestException("Outlet-to-plant transfer requires FromBranchId and ToPlantId.");
                }

                if (fromPlantId.HasValue || toBranchId.HasValue)
                {
                    throw new BadRequestException("Outlet-to-plant transfer should not include FromPlantId or ToBranchId.");
                }

                return;
            }

            if (transferTypeCode == TransferTypeCodes.PlantToOutlet)
            {
                if (!fromPlantId.HasValue || !toBranchId.HasValue)
                {
                    throw new BadRequestException("Plant-to-outlet transfer requires FromPlantId and ToBranchId.");
                }

                if (fromBranchId.HasValue || toPlantId.HasValue)
                {
                    throw new BadRequestException("Plant-to-outlet transfer should not include FromBranchId or ToPlantId.");
                }

                return;
            }

            throw new BadRequestException($"Unsupported transfer type '{transferTypeCode}'.");
        }

        private async Task ValidateLocationsAsync(
            CreateTransferBatchCommand request,
            string transferTypeCode,
            CancellationToken cancellationToken)
        {
            if (transferTypeCode == TransferTypeCodes.OutletToPlant)
            {
                var branch = await _dbContext.Branches
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == request.FromBranchId!.Value, cancellationToken);

                if (branch is null)
                {
                    throw new NotFoundException("Branch", request.FromBranchId!.Value);
                }

                if (!branch.IsActive)
                {
                    throw new BadRequestException("Selected branch is inactive.");
                }

                var plant = await _dbContext.Plants
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == request.ToPlantId!.Value, cancellationToken);

                if (plant is null)
                {
                    throw new NotFoundException("Plant", request.ToPlantId!.Value);
                }

                if (!plant.IsActive)
                {
                    throw new BadRequestException("Selected plant is inactive.");
                }
            }

            if (transferTypeCode == TransferTypeCodes.PlantToOutlet)
            {
                var plant = await _dbContext.Plants
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == request.FromPlantId!.Value, cancellationToken);

                if (plant is null)
                {
                    throw new NotFoundException("Plant", request.FromPlantId!.Value);
                }

                if (!plant.IsActive)
                {
                    throw new BadRequestException("Selected plant is inactive.");
                }

                var branch = await _dbContext.Branches
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == request.ToBranchId!.Value, cancellationToken);

                if (branch is null)
                {
                    throw new NotFoundException("Branch", request.ToBranchId!.Value);
                }

                if (!branch.IsActive)
                {
                    throw new BadRequestException("Selected branch is inactive.");
                }
            }
        }

        private async Task ValidateDriverAsync(
            int driverUserId,
            CancellationToken cancellationToken)
        {
            var driver = await _identityService.FindByUserIdAsync(
                driverUserId,
                cancellationToken);

            if (driver is null)
            {
                throw new NotFoundException("Driver user", driverUserId);
            }

            if (!driver.IsActive)
            {
                throw new BadRequestException("Selected driver user is inactive.");
            }

            var roles = await _identityService.GetRolesAsync(
                driverUserId,
                cancellationToken);

            if (!roles.Contains(RoleCodes.Driver))
            {
                throw new BadRequestException("Selected user does not have the DRIVER role.");
            }
        }

        private async Task ValidateOrdersForTransferAsync(
            IReadOnlyCollection<Order> orders,
            string transferTypeCode,
            int? fromBranchId,
            int? fromPlantId,
            CancellationToken cancellationToken)
        {
            if (transferTypeCode == TransferTypeCodes.OutletToPlant)
            {
                foreach (var order in orders)
                {
                    if (order.BranchId != fromBranchId!.Value)
                    {
                        throw new BadRequestException($"Order '{order.OrderNo}' does not belong to the selected branch.");
                    }

                    var statusCode = LookupValueHelper.GetCode(order.CurrentStatus);

                    if (statusCode != OrderStatusCodes.Created)
                    {
                        throw new ConflictException($"Order '{order.OrderNo}' must be in Created status before sending to plant.");
                    }
                }

                return;
            }

            if (transferTypeCode == TransferTypeCodes.PlantToOutlet)
            {
                foreach (var order in orders)
                {
                    var statusCode = LookupValueHelper.GetCode(order.CurrentStatus);

                    if (statusCode != OrderStatusCodes.ReadyForOutletReturn)
                    {
                        throw new ConflictException($"Order '{order.OrderNo}' must be Ready for Outlet Return before plant-to-outlet transfer.");
                    }
                }

                return;
            }

            throw new BadRequestException($"Unsupported transfer type '{transferTypeCode}'.");
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

        private void ChangeOrderStatus(
            Order order,
            OrderStatus newStatus,
            string remarks)
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
}
