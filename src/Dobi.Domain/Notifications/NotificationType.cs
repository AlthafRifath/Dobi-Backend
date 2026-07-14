using Dobi.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Notifications
{
    public class NotificationType : BaseEntity
    {
        public string TypeCode { get; set; } = string.Empty; // SMS
        public string TypeName { get; set; } = string.Empty;

        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
