using Dobi.Contracts.Notifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Notifications.RetryNotification
{
    public sealed record RetryNotificationCommand(
        int NotificationId) : IRequest<NotificationResponse>;
}
