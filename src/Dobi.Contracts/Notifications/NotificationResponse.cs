using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Notifications
{
    public sealed record NotificationResponse(
        int NotificationId,
        int? OrderId,
        int? CustomerId,
        int NotificationTypeId,
        string NotificationTypeName,
        string Recipient,
        string Message,
        int NotificationStatusId,
        string NotificationStatusName,
        DateTime? SentAt,
        string? ErrorMessage);
}
