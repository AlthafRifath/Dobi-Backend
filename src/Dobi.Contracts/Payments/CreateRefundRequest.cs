using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Payments
{
    public sealed record CreateRefundRequest(
        int OrderId,
        int? PaymentId,
        decimal Amount,
        string Reason);
}
