using Dobi.Domain.Common;
using Dobi.Domain.Notifications;
using Dobi.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Customers
{
    public class Customer : AuditableEntity
    {
        public string CustomerNo { get; set; } = string.Empty;

        public int CustomerTypeId { get; set; }
        public CustomerType CustomerType { get; set; } = null!;

        public string FullName { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? Email { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
