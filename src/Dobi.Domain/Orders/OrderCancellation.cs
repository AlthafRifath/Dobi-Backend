using Dobi.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Orders
{
    public class OrderCancellation : BaseEntity
    {
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public string CancellationReason { get; set; } = string.Empty;
        public bool RequestedByCustomer { get; set; }

        public int CancelledByUserId { get; set; }
        public DateTime CancelledAt { get; set; }
    }
}
