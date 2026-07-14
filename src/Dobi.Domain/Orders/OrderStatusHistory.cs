using Dobi.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Orders
{
    public class OrderStatusHistory : BaseEntity
    {
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public int OrderStatusId { get; set; }
        public OrderStatus OrderStatus { get; set; } = null!;

        public string? Remarks { get; set; }

        public int ChangedByUserId { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}
