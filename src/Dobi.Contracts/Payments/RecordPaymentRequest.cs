using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Payments
{
    public sealed record RecordPaymentRequest(
        decimal Amount,
        int PaymentMethodId,
        string? ReferenceNo,
        string? ChequeNo,
        string? ChequeBankName,
        DateOnly? ChequeDate);
}
