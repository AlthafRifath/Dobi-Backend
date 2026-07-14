using Dobi.Domain.Common;
using Dobi.Domain.Inspections;
using Dobi.Domain.PlantProcessing;
using Dobi.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Orders
{
    public class OrderItem : AuditableEntity
    {
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public int ServiceId { get; set; }
        public Service Service { get; set; } = null!;

        public int ItemCategoryId { get; set; }
        public ItemCategory ItemCategory { get; set; } = null!;

        public int? ServicePriceId { get; set; }
        public ServicePrice? ServicePrice { get; set; }

        public int PricingTypeId { get; set; }
        public PricingType PricingType { get; set; } = null!;

        public int Quantity { get; set; }
        public decimal? WeightKg { get; set; }

        public decimal UnitPrice { get; set; }
        public decimal LineAmount { get; set; }

        public string? SpecialNotes { get; set; }

        public ICollection<OrderItemTag> Tags { get; set; } = new List<OrderItemTag>();
        public ICollection<InspectionRecord> InspectionRecords { get; set; } = new List<InspectionRecord>();
        public ICollection<QCRecord> QCRecords { get; set; } = new List<QCRecord>();
    }
}
