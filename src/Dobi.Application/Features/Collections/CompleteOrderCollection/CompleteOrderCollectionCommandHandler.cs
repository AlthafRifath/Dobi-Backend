using Dobi.Application.Abstractions.Persistence;
using Dobi.Application.Abstractions.Services;
using Dobi.Application.Common;
using Dobi.Contracts.Collections;
using Dobi.Domain.Collections;
using Dobi.Domain.Orders;
using Dobi.Shared.Constants;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Collections.CompleteOrderCollection
{
    public sealed class CompleteOrderCollectionCommandHandler
    : IRequestHandler<CompleteOrderCollectionCommand, OrderCollectionResponse>
    {
        private const string CollectionModeCollectionCode = "COLLECTION";
        private const string CollectionModeDeliveryCode = "DELIVERY";

        private readonly IDobiDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CompleteOrderCollectionCommandHandler(
            IDobiDbContext dbContext,
            ICurrentUserService currentUserService,
            IDateTimeProvider dateTimeProvider)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<OrderCollectionResponse> Handle(
            CompleteOrderCollectionCommand request,
            CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId is null)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }

            var order = await _dbContext.Orders
                .Include(x => x.Customer)
                    .ThenInclude(x => x.CustomerType)
                .Include(x => x.Branch)
                .Include(x => x.CurrentStatus)
                .Include(x => x.PaymentStatus)
                .Include(x => x.Collection)
                .FirstOrDefaultAsync(x => x.Id == request.OrderId, cancellationToken);

            if (order is null)
            {
                throw new NotFoundException("Order", request.OrderId);
            }

            if (order.Collection is not null)
            {
                throw new ConflictException("Order has already been collected or delivered.");
            }

            var currentStatusCode = LookupValueHelper.GetCode(order.CurrentStatus);

            if (currentStatusCode != OrderStatusCodes.ReadyForCollection)
            {
                throw new ConflictException("Order must be ready for collection or delivery before completing this action.");
            }

            var customerTypeCode = LookupValueHelper.GetCode(order.Customer.CustomerType);

            var collectionModeCode = customerTypeCode == CustomerTypeCodes.B2C
                ? CollectionModeCollectionCode
                : CollectionModeDeliveryCode;

            var collectionMode = await GetCollectionModeByCodeAsync(
                collectionModeCode,
                cancellationToken);

            var collectorName = string.IsNullOrWhiteSpace(request.CollectorName)
                ? null
                : request.CollectorName.Trim();

            var collectorMobileNo = string.IsNullOrWhiteSpace(request.CollectorMobileNo)
                ? null
                : request.CollectorMobileNo.Trim();

            var isCollectedByCustomer = customerTypeCode == CustomerTypeCodes.B2C &&
                                        request.IsCollectedByCustomer;

            if (customerTypeCode == CustomerTypeCodes.B2C)
            {
                if (!request.ReceiptVerified && !request.MobileNoVerified)
                {
                    throw new BadRequestException("For B2C collection, receipt or mobile number must be verified.");
                }

                if (isCollectedByCustomer)
                {
                    collectorName ??= order.Customer.FullName;
                    collectorMobileNo ??= order.Customer.MobileNo;
                }
            }
            else
            {
                isCollectedByCustomer = false;

                if (string.IsNullOrWhiteSpace(collectorName))
                {
                    throw new BadRequestException("For B2B/Bulk delivery, receiver or collector name is required.");
                }
            }

            var collectedDeliveredStatus = await GetOrderStatusByCodeAsync(
                OrderStatusCodes.CollectedDelivered,
                cancellationToken);

            var now = _dateTimeProvider.UtcNow;

            var orderCollection = new OrderCollection
            {
                OrderId = order.Id,
                CollectionModeId = collectionMode.Id,
                IsCollectedByCustomer = isCollectedByCustomer,
                CollectorName = collectorName,
                CollectorMobileNo = collectorMobileNo,
                ReceiptVerified = request.ReceiptVerified,
                MobileNoVerified = request.MobileNoVerified,
                ReceiptImageUrl = string.IsNullOrWhiteSpace(request.ReceiptImageUrl)
                    ? null
                    : request.ReceiptImageUrl.Trim(),
                CustomerSignatureUrl = string.IsNullOrWhiteSpace(request.CustomerSignatureUrl)
                    ? null
                    : request.CustomerSignatureUrl.Trim(),
                CollectedOrDeliveredAt = now,
                ReleasedByUserId = _currentUserService.UserId.Value,
                Remarks = string.IsNullOrWhiteSpace(request.Remarks)
                    ? null
                    : request.Remarks.Trim()
            };

            _dbContext.OrderCollections.Add(orderCollection);

            order.CurrentStatusId = collectedDeliveredStatus.Id;
            order.UpdatedAt = now;
            order.UpdatedByUserId = _currentUserService.UserId;

            _dbContext.OrderStatusHistories.Add(new OrderStatusHistory
            {
                OrderId = order.Id,
                OrderStatusId = collectedDeliveredStatus.Id,
                Remarks = customerTypeCode == CustomerTypeCodes.B2C
                    ? "Order collected from outlet."
                    : "Order delivered to customer office/address.",
                ChangedByUserId = _currentUserService.UserId.Value,
                ChangedAt = now
            });

            await _dbContext.SaveChangesAsync(cancellationToken);

            var savedOrder = await LoadOrderAsync(order.Id, cancellationToken);

            return OrderCollectionResponseMapper.Map(savedOrder);
        }

        private async Task<CollectionMode> GetCollectionModeByCodeAsync(
            string collectionModeCode,
            CancellationToken cancellationToken)
        {
            var modes = await _dbContext.CollectionModes
                .ToListAsync(cancellationToken);

            var mode = modes.FirstOrDefault(x =>
                LookupValueHelper.GetCode(x) == collectionModeCode);

            if (mode is null)
            {
                throw new InvalidOperationException($"Collection mode '{collectionModeCode}' was not found.");
            }

            return mode;
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

        private async Task<Order> LoadOrderAsync(
            int orderId,
            CancellationToken cancellationToken)
        {
            return await _dbContext.Orders
                .AsNoTracking()
                .Include(x => x.Customer)
                    .ThenInclude(x => x.CustomerType)
                .Include(x => x.Branch)
                .Include(x => x.CurrentStatus)
                .Include(x => x.PaymentStatus)
                .Include(x => x.Collection)
                    .ThenInclude(x => x!.CollectionMode)
                .FirstAsync(x => x.Id == orderId, cancellationToken);
        }
    }
}
