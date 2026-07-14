using Dobi.Domain.Common;
using Dobi.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Services
{
    public class Service : AuditableEntity
    {
        public string ServiceCode { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public string? Description { get; set; }

        public bool IsExpressEligible { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<ServicePrice> ServicePrices { get; set; } = new List<ServicePrice>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
