using Dobi.Application.Common;
using Dobi.Contracts.Notifications;
using Dobi.Domain.Notifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Notifications
{
    internal static class NotificationResponseMapper
    {
        public static NotificationResponse Map(Notification notification)
        {
            return new NotificationResponse(
                notification.Id,

                notification.OrderId,
                notification.Order?.OrderNo,

                notification.CustomerId,
                notification.Customer.FullName,
                notification.Customer.MobileNo,

                notification.NotificationTypeId,
                LookupValueHelper.GetCode(notification.NotificationType),
                LookupValueHelper.GetName(notification.NotificationType),

                notification.NotificationStatusId,
                LookupValueHelper.GetCode(notification.NotificationStatus),
                LookupValueHelper.GetName(notification.NotificationStatus),

                notification.Recipient,
                notification.Message,

                notification.SentAt,
                notification.ErrorMessage,

                notification.CreatedAt,
                notification.CreatedByUserId);
        }
    }
}
