using Dobi.Contracts.Notifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Notifications.GetNotificationsByOrder
{
    public sealed record GetNotificationsByOrderQuery(
        int OrderId) : IRequest<IReadOnlyCollection<NotificationResponse>>;
}
