using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Notifications
{
    public sealed record NotificationResponse(
        int NotificationId,

        int? OrderId,
        string? OrderNo,

        int? CustomerId,
        string CustomerName,
        string CustomerMobileNo,

        int NotificationTypeId,
        string NotificationTypeCode,
        string NotificationTypeName,

        int NotificationStatusId,
        string NotificationStatusCode,
        string NotificationStatusName,

        string Recipient,
        string Message,

        DateTime? SentAt,
        string? ErrorMessage,

        DateTime CreatedAt,
        int? CreatedByUserId);
}
