using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Payments
{
    public sealed record RecordPaymentRequest(
        int OrderId,
        decimal Amount,
        int PaymentMethodId,
        string? ReferenceNo,
        DateTime? PaidAt);
}
