using Dobi.Application.Abstractions.Persistence;
using Dobi.Application.Abstractions.Services;
using Dobi.Application.Common;
using Dobi.Contracts.PlantProcessing;
using Dobi.Domain.Orders;
using Dobi.Shared.Constants;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PlantProcessingEntity = Dobi.Domain.PlantProcessing.PlantProcessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.PlantProcessing.MarkReadyForOutletReturn
{
    public sealed class MarkReadyForOutletReturnCommandHandler
    : IRequestHandler<MarkReadyForOutletReturnCommand, PlantProcessingResponse>
    {
        private readonly IDobiDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public MarkReadyForOutletReturnCommandHandler(
            IDobiDbContext dbContext,
            ICurrentUserService currentUserService,
            IDateTimeProvider dateTimeProvider)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<PlantProcessingResponse> Handle(
            MarkReadyForOutletReturnCommand request,
            CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId is null)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }

            var plantProcessing = await _dbContext.PlantProcessings
                .Include(x => x.Order)
                    .ThenInclude(x => x.CurrentStatus)
                .Include(x => x.Order)
                    .ThenInclude(x => x.Customer)
                        .ThenInclude(x => x.CustomerType)
                .FirstOrDefaultAsync(x => x.Id == request.PlantProcessingId, cancellationToken);

            if (plantProcessing is null)
            {
                throw new NotFoundException("Plant processing", request.PlantProcessingId);
            }

            var orderStatusCode = LookupValueHelper.GetCode(plantProcessing.Order.CurrentStatus);

            if (orderStatusCode != OrderStatusCodes.Packed)
            {
                throw new ConflictException("Only packed orders can be marked ready for outlet return.");
            }

            var customerTypeCode = LookupValueHelper.GetCode(
                plantProcessing.Order.Customer.CustomerType);

            if (customerTypeCode != CustomerTypeCodes.B2C)
            {
                throw new ConflictException(
                    "Only B2C orders can be marked ready for outlet return. B2B/Bulk orders must use the customer delivery workflow.");
            }

            var readyForOutletReturnStatus = await GetOrderStatusByCodeAsync(
                OrderStatusCodes.ReadyForOutletReturn,
                cancellationToken);

            var now = _dateTimeProvider.UtcNow;

            plantProcessing.ReadyDate = request.ReadyDate;

            if (!string.IsNullOrWhiteSpace(request.PlantRemarks))
            {
                plantProcessing.PlantRemarks = request.PlantRemarks.Trim();
            }

            plantProcessing.UpdatedAt = now;
            plantProcessing.UpdatedByUserId = _currentUserService.UserId;

            plantProcessing.Order.CurrentStatusId = readyForOutletReturnStatus.Id;
            plantProcessing.Order.UpdatedAt = now;
            plantProcessing.Order.UpdatedByUserId = _currentUserService.UserId;

            _dbContext.OrderStatusHistories.Add(new OrderStatusHistory
            {
                OrderId = plantProcessing.OrderId,
                OrderStatusId = readyForOutletReturnStatus.Id,
                Remarks = "B2C order ready for outlet return.",
                ChangedByUserId = _currentUserService.UserId.Value,
                ChangedAt = now
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
