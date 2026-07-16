using Dobi.Contracts.Notifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Notifications.SendSmsNotification
{
    public sealed record SendSmsNotificationCommand(
        int? OrderId,
        int? CustomerId,
        string? RecipientMobileNo,
        string Message) : IRequest<NotificationResponse>;
}
