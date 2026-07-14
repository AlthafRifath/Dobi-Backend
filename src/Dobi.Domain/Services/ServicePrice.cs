using Dobi.Domain.Common;
using Dobi.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Services
{
    public class ServicePrice : AuditableEntity
    {
        public int ServiceId { get; set; }
        public Service Service { get; set; } = null!;

        public int? ItemCategoryId { get; set; }
        public ItemCategory? ItemCategory { get; set; }

        public int PricingTypeId { get; set; }
        public PricingType PricingType { get; set; } = null!;

        public decimal BasePrice { get; set; }
        public decimal? ExpressAdditionalPrice { get; set; }

        public DateOnly EffectiveFrom { get; set; }
        public DateOnly? EffectiveTo { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
