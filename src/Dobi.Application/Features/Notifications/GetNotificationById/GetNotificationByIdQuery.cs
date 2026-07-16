using Dobi.Contracts.Notifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Notifications.GetNotificationById
{
    public sealed record GetNotificationByIdQuery(
        int NotificationId) : IRequest<NotificationResponse>;
}
