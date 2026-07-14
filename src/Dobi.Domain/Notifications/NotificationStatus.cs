using Dobi.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Notifications
{
    public class NotificationStatus : BaseEntity
    {
        public string StatusCode { get; set; } = string.Empty; // PENDING, SENT, FAILED
        public string StatusName { get; set; } = string.Empty;

        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
