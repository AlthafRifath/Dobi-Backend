using Dobi.Domain.Common;
using Dobi.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Payments
{
    public class Payment : AuditableEntity
    {
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public decimal Amount { get; set; }

        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; } = null!;

        public int PaymentStatusId { get; set; }
        public PaymentStatus PaymentStatus { get; set; } = null!;

        public DateTime? PaidAt { get; set; }
        public int? ReceivedByUserId { get; set; }

        public string? ReferenceNo { get; set; }

        public string? ChequeNo { get; set; }
        public string? ChequeBankName { get; set; }
        public DateOnly? ChequeDate { get; set; }

        public DateTime? ChequeClearedAt { get; set; }
        public DateTime? ChequeRejectedAt { get; set; }
        public string? ChequeRejectionReason { get; set; }

        public ICollection<Refund> Refunds { get; set; } = new List<Refund>();
    }
}
