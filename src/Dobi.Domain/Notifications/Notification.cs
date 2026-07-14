using Dobi.Domain.Common;
using Dobi.Domain.Customers;
using Dobi.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Notifications
{
    public class Notification : AuditableEntity
    {
        public int? OrderId { get; set; }
        public Order? Order { get; set; }

        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public int NotificationTypeId { get; set; }
        public NotificationType NotificationType { get; set; } = null!;

        public string Recipient { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public int NotificationStatusId { get; set; }
        public NotificationStatus NotificationStatus { get; set; } = null!;

        public DateTime? SentAt { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
