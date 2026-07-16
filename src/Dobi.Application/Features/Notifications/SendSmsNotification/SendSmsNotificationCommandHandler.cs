using Dobi.Application.Abstractions.Persistence;
using Dobi.Application.Abstractions.Services;
using Dobi.Application.Common;
using Dobi.Contracts.Notifications;
using Dobi.Domain.Customers;
using Dobi.Domain.Notifications;
using Dobi.Domain.Orders;
using Dobi.Shared.Constants;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Notifications.SendSmsNotification
{
    public sealed class SendSmsNotificationCommandHandler
    : IRequestHandler<SendSmsNotificationCommand, NotificationResponse>
    {
        private readonly IDobiDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ISmsSender _smsSender;

        public SendSmsNotificationCommandHandler(
            IDobiDbContext dbContext,
            ICurrentUserService currentUserService,
            IDateTimeProvider dateTimeProvider,
            ISmsSender smsSender)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
            _smsSender = smsSender;
        }

        public async Task<NotificationResponse> Handle(
            SendSmsNotificationCommand request,
            CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId is null)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }

            Order? order = null;
            Customer customer;

            if (request.OrderId.HasValue)
            {
                order = await _dbContext.Orders
                    .Include(x => x.Customer)
                    .FirstOrDefaultAsync(x => x.Id == request.OrderId.Value, cancellationToken);

                if (order is null)
                {
                    throw new NotFoundException("Order", request.OrderId.Value);
                }

                customer = order.Customer;
            }
            else
            {
                customer = await _dbContext.Customers
                    .FirstOrDefaultAsync(x => x.Id == request.CustomerId!.Value, cancellationToken)
                    ?? throw new NotFoundException("Customer", request.CustomerId!.Value);
            }

            if (!customer.IsActive)
            {
                throw new BadRequestException("Customer is inactive.");
            }

            var recipient = string.IsNullOrWhiteSpace(request.RecipientMobileNo)
                ? customer.MobileNo
                : request.RecipientMobileNo.Trim();

            var smsType = await GetNotificationTypeByCodeAsync(
                NotificationTypeCodes.Sms,
                cancellationToken);

            var pendingStatus = await GetNotificationStatusByCodeAsync(
                NotificationStatusCodes.Pending,
                cancellationToken);

            var now = _dateTimeProvider.UtcNow;

            var notification = new Notification
            {
                OrderId = order?.Id,
                CustomerId = customer.Id,
                NotificationTypeId = smsType.Id,
                NotificationStatusId = pendingStatus.Id,
                Recipient = recipient,
                Message = request.Message.Trim(),
                SentAt = null,
                ErrorMessage = null,
                CreatedAt = now,
                CreatedByUserId = _currentUserService.UserId
            };

            _dbContext.Notifications.Add(notification);

            await _dbContext.SaveChangesAsync(cancellationToken);

            await SendAndUpdateStatusAsync(notification, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            var savedNotification = await LoadNotificationAsync(notification.Id, cancellationToken);

            return NotificationResponseMapper.Map(savedNotification);
        }

        private async Task SendAndUpdateStatusAsync(
            Notification notification,
            CancellationToken cancellationToken)
        {
            var result = await _smsSender.SendAsync(
                notification.Recipient,
                notification.Message,
                cancellationToken);

            if (result.IsSuccess)
            {
                var sentStatus = await GetNotificationStatusByCodeAsync(
                    NotificationStatusCodes.Sent,
                    cancellationToken);

                notification.NotificationStatusId = sentStatus.Id;
                notification.SentAt = _dateTimeProvider.UtcNow;
                notification.ErrorMessage = null;
            }
            else
            {
                var failedStatus = await GetNotificationStatusByCodeAsync(
                    NotificationStatusCodes.Failed,
                    cancellationToken);

                notification.NotificationStatusId = failedStatus.Id;
                notification.SentAt = null;
                notification.ErrorMessage = result.ErrorMessage;
            }

            notification.UpdatedAt = _dateTimeProvider.UtcNow;
            notification.UpdatedByUserId = _currentUserService.UserId;
        }

        private async Task<NotificationType> GetNotificationTypeByCodeAsync(
            string typeCode,
            CancellationToken cancellationToken)
        {
            var types = await _dbContext.NotificationTypes
                .ToListAsync(cancellationToken);

            var type = types.FirstOrDefault(x =>
                LookupValueHelper.GetCode(x) == typeCode);

            if (type is null)
            {
                throw new InvalidOperationException($"Notification type '{typeCode}' was not found.");
            }

            return type;
        }

        private async Task<NotificationStatus> GetNotificationStatusByCodeAsync(
            string statusCode,
            CancellationToken cancellationToken)
        {
            var statuses = await _dbContext.NotificationStatuses
                .ToListAsync(cancellationToken);

            var status = statuses.FirstOrDefault(x =>
                LookupValueHelper.GetCode(x) == statusCode);

            if (status is null)
            {
                throw new InvalidOperationException($"Notification status '{statusCode}' was not found.");
            }

            return status;
        }

        private async Task<Notification> LoadNotificationAsync(
            int notificationId,
            CancellationToken cancellationToken)
        {
            return await _dbContext.Notifications
                .AsNoTracking()
                .Include(x => x.Order)
                .Include(x => x.Customer)
                .Include(x => x.NotificationType)
                .Include(x => x.NotificationStatus)
                .FirstAsync(x => x.Id == notificationId, cancellationToken);
        }
    }
}
