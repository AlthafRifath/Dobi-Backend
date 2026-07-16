using Dobi.Application.Abstractions.Persistence;
using Dobi.Application.Abstractions.Services;
using Dobi.Application.Common;
using Dobi.Contracts.Collections;
using Dobi.Domain.Orders;
using Dobi.Shared.Constants;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Collections.MarkOrderReadyForCollection
{
    public sealed class MarkOrderReadyForCollectionCommandHandler
    : IRequestHandler<MarkOrderReadyForCollectionCommand, OrderCollectionResponse>
    {
        private readonly IDobiDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public MarkOrderReadyForCollectionCommandHandler(
            IDobiDbContext dbContext,
            ICurrentUserService currentUserService,
            IDateTimeProvider dateTimeProvider)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<OrderCollectionResponse> Handle(
            MarkOrderReadyForCollectionCommand request,
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
                    .ThenInclude(x => x!.CollectionMode)
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
            var customerTypeCode = LookupValueHelper.GetCode(order.Customer.CustomerType);

            if (currentStatusCode == OrderStatusCodes.ReadyForCollection)
            {
                return OrderCollectionResponseMapper.Map(order);
            }

            if (customerTypeCode == CustomerTypeCodes.B2C)
            {
                if (currentStatusCode != OrderStatusCodes.ReturnedToOutlet)
                {
                    throw new ConflictException("B2C orders must be returned to outlet before they can be marked ready for collection.");
                }
            }
            else
            {
                if (currentStatusCode != OrderStatusCodes.Packed)
                {
                    throw new ConflictException("B2B/Bulk orders must be packed before they can be marked ready for customer delivery.");
                }
            }

            var readyForCollectionStatus = await GetOrderStatusByCodeAsync(
                OrderStatusCodes.ReadyForCollection,
                cancellationToken);

            var remarks = string.IsNullOrWhiteSpace(request.Remarks)
                ? customerTypeCode == CustomerTypeCodes.B2C
                    ? "B2C order ready for customer collection at outlet."
                    : "B2B/Bulk order ready for customer office delivery."
                : request.Remarks.Trim();

            order.CurrentStatusId = readyForCollectionStatus.Id;
            order.UpdatedAt = _dateTimeProvider.UtcNow;
            order.UpdatedByUserId = _currentUserService.UserId;

            _dbContext.OrderStatusHistories.Add(new OrderStatusHistory
            {
                OrderId = order.Id,
                OrderStatusId = readyForCollectionStatus.Id,
                Remarks = remarks,
                ChangedByUserId = _currentUserService.UserId.Value,
                ChangedAt = _dateTimeProvider.UtcNow
            });

            await _dbContext.SaveChangesAsync(cancellationToken);

            var savedOrder = await LoadOrderAsync(order.Id, cancellationToken);

            return OrderCollectionResponseMapper.Map(savedOrder);
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
