using Dobi.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Payments
{
    public class RefundStatus : BaseEntity
    {
        public string StatusCode { get; set; } = string.Empty; // PENDING, APPROVED, PAID, REJECTED
        public string StatusName { get; set; } = string.Empty;

        public ICollection<Refund> Refunds { get; set; } = new List<Refund>();
    }
}
