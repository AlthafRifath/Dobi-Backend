using Dobi.Domain.Common;
using Dobi.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Payments
{
    public class Refund : AuditableEntity
    {
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public int? PaymentId { get; set; }
        public Payment? Payment { get; set; }

        public decimal Amount { get; set; }
        public string Reason { get; set; } = string.Empty;

        public int RefundStatusId { get; set; }
        public RefundStatus RefundStatus { get; set; } = null!;

        public int? ApprovedByUserId { get; set; }
        public DateTime? RefundedAt { get; set; }

        public int? RejectedByUserId { get; set; }
        public DateTime? RejectedAt { get; set; }
        public string? RejectionReason { get; set; }
    }
}
