using Dobi.Application.Abstractions.Persistence;
using Dobi.Application.Abstractions.Services;
using Dobi.Application.Common;
using Dobi.Contracts.Notifications;
using Dobi.Domain.Notifications;
using Dobi.Shared.Constants;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Notifications.RetryNotification
{
    public sealed class RetryNotificationCommandHandler
    : IRequestHandler<RetryNotificationCommand, NotificationResponse>
    {
        private readonly IDobiDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ISmsSender _smsSender;

        public RetryNotificationCommandHandler(
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
            RetryNotificationCommand request,
            CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId is null)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }

            var notification = await _dbContext.Notifications
                .Include(x => x.NotificationType)
                .Include(x => x.NotificationStatus)
                .FirstOrDefaultAsync(x => x.Id == request.NotificationId, cancellationToken);

            if (notification is null)
            {
                throw new NotFoundException("Notification", request.NotificationId);
            }

            var typeCode = LookupValueHelper.GetCode(notification.NotificationType);

            if (typeCode != NotificationTypeCodes.Sms)
            {
                throw new ConflictException("Only SMS notifications can be retried.");
            }

            var statusCode = LookupValueHelper.GetCode(notification.NotificationStatus);

            if (statusCode == NotificationStatusCodes.Sent)
            {
                throw new ConflictException("Sent notifications cannot be retried.");
            }

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

            await _dbContext.SaveChangesAsync(cancellationToken);

            var savedNotification = await LoadNotificationAsync(notification.Id, cancellationToken);

            return NotificationResponseMapper.Map(savedNotification);
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
