using Dobi.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Payments
{
    public class PaymentMethod : BaseEntity
    {
        public string MethodCode { get; set; } = string.Empty; // CASH, CARD, BANK_TRANSFER
        public string MethodName { get; set; } = string.Empty;

        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
