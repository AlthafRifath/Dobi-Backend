using Dobi.Domain.Common;
using Dobi.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Orders
{
    public class PaymentStatus : BaseEntity
    {
        public string StatusCode { get; set; } = string.Empty;
        public string StatusName { get; set; } = string.Empty;

        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
