using Dobi.Domain.Common;
using Dobi.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Inspections
{
    public class InspectionRecord : AuditableEntity
    {
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public int? OrderItemId { get; set; }
        public OrderItem? OrderItem { get; set; }

        public bool CustomerAcknowledged { get; set; }
        public string? CustomerSignatureUrl { get; set; }
        public string? Notes { get; set; }

        public int InspectedByUserId { get; set; }
        public DateTime InspectedAt { get; set; }

        public ICollection<InspectionIssue> Issues { get; set; } = new List<InspectionIssue>();
        public ICollection<InspectionPhoto> Photos { get; set; } = new List<InspectionPhoto>();
    }
}
