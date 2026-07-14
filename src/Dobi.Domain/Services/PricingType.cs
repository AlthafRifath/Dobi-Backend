using Dobi.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Services
{
    public class PricingType : BaseEntity
    {
        public string PricingTypeCode { get; set; } = string.Empty; // PER_KG, PER_ITEM
        public string PricingTypeName { get; set; } = string.Empty;

        public ICollection<ItemCategory> ItemCategories { get; set; } = new List<ItemCategory>();
        public ICollection<ServicePrice> ServicePrices { get; set; } = new List<ServicePrice>();
    }
}
