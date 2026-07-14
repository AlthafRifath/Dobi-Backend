using Dobi.Domain.Common;
using Dobi.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Services
{
    public class ItemCategory : AuditableEntity
    {
        public string CategoryName { get; set; } = string.Empty;

        public int DefaultPricingTypeId { get; set; }
        public PricingType DefaultPricingType { get; set; } = null!;

        public bool IsSpecialItem { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<ServicePrice> ServicePrices { get; set; } = new List<ServicePrice>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
