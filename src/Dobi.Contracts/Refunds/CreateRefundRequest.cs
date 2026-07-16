using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Refunds
{
    public sealed record CreateRefundRequest(
        int? PaymentId,
        decimal Amount,
        string Reason);
}
