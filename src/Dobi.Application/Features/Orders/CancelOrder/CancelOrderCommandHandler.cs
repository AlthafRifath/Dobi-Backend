using Dobi.Application.Abstractions.Persistence;
using Dobi.Application.Abstractions.Services;
using Dobi.Contracts.Orders;
using Dobi.Domain.Orders;
using Dobi.Shared.Constants;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Orders.CancelOrder
{
    public sealed class CancelOrderCommandHandler
    : IRequestHandler<CancelOrderCommand, OrderResponse>
    {
        private readonly IDobiDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CancelOrderCommandHandler(
            IDobiDbContext dbContext,
            ICurrentUserService currentUserService,
            IDateTimeProvider dateTimeProvider)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<OrderResponse> Handle(
            CancelOrderCommand request,
            CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId is null)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }

            var order = await _dbContext.Orders
                .Include(x => x.Customer)
                .Include(x => x.Branch)
                .Include(x => x.CurrentStatus)
                .Include(x => x.PaymentStatus)
                .Include(x => x.Items)
                    .ThenInclude(x => x.Service)
                .Include(x => x.Items)
                    .ThenInclude(x => x.ItemCategory)
                .Include(x => x.Items)
                    .ThenInclude(x => x.PricingType)
                .Include(x => x.Items)
                    .ThenInclude(x => x.Tags)
                .FirstOrDefaultAsync(x => x.Id == request.OrderId, cancellationToken);

            if (order is null)
            {
                throw new NotFoundException("Order", request.OrderId);
            }

            var currentStatusCode = order.CurrentStatus.StatusCode;

            var allowedStatuses = new[]
            {
            OrderStatusCodes.Draft,
            OrderStatusCodes.Created
        };

            if (!allowedStatuses.Contains(currentStatusCode))
            {
                throw new ConflictException("Order cannot be cancelled after it has been sent to plant or processing has started.");
            }

            var cancelledStatus = await _dbContext.OrderStatuses
                .FirstOrDefaultAsync(x => x.StatusCode == OrderStatusCodes.Cancelled, cancellationToken);

            if (cancelledStatus is null)
            {
                throw new InvalidOperationException("Cancelled order status was not found.");
            }

            var alreadyCancelled = await _dbContext.OrderCancellations.AnyAsync(
                x => x.OrderId == order.Id,
                cancellationToken);

            if (alreadyCancelled)
            {
                throw new ConflictException("Order is already cancelled.");
            }

            order.CurrentStatusId = cancelledStatus.Id;
            order.UpdatedAt = _dateTimeProvider.UtcNow;
            order.UpdatedByUserId = _currentUserService.UserId;

            _dbContext.OrderCancellations.Add(new OrderCancellation
            {
                OrderId = order.Id,
                CancellationReason = request.CancellationReason.Trim(),
                RequestedByCustomer = request.RequestedByCustomer,
                CancelledByUserId = _currentUserService.UserId.Value,
                CancelledAt = _dateTimeProvider.UtcNow
            });

            _dbContext.OrderStatusHistories.Add(new OrderStatusHistory
            {
                OrderId = order.Id,
                OrderStatusId = cancelledStatus.Id,
                Remarks = request.CancellationReason.Trim(),
                ChangedByUserId = _currentUserService.UserId.Value,
                ChangedAt = _dateTimeProvider.UtcNow
            });

            await _dbContext.SaveChangesAsync(cancellationToken);

            order.CurrentStatus = cancelledStatus;

            return new OrderResponse(
                order.Id,
                order.OrderNo,
                order.CustomerId,
                order.Customer.FullName,
                order.Customer.MobileNo,
                order.BranchId,
                order.Branch.BranchName,
                order.OrderDate,
                order.ExpectedReturnDate,
                order.CurrentStatusId,
                cancelledStatus.StatusName,
                order.PaymentStatusId,
                order.PaymentStatus.StatusName,
                order.IsExpress,
                order.SubTotalAmount,
                order.ExpressChargeAmount,
                order.TotalAmount,
                order.Items
                    .OrderBy(x => x.Id)
                    .Select(x => new OrderItemResponse(
                        x.Id,
                        x.ServiceId,
                        x.Service.ServiceName,
                        x.ItemCategoryId,
                        x.ItemCategory.CategoryName,
                        x.PricingTypeId,
                        x.PricingType.PricingTypeName,
                        x.Quantity,
                        x.WeightKg,
                        x.UnitPrice,
                        x.LineAmount,
                        x.SpecialNotes,
                        x.Tags.Select(t => t.TagNo).ToArray()))
                    .ToArray());
        }
    }
}
