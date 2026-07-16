using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Notifications
{
    public sealed record SendSmsNotificationRequest(
        int? OrderId,
        int? CustomerId,
        string? RecipientMobileNo,
        string Message);
}
