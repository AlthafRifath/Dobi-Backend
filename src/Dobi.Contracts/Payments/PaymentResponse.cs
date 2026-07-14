using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Payments
{
    public sealed record PaymentResponse(
        int PaymentId,
        int OrderId,
        string OrderNo,
        decimal Amount,
        int PaymentMethodId,
        string PaymentMethodName,
        int PaymentStatusId,
        string PaymentStatusName,
        DateTime? PaidAt,
        int? ReceivedByUserId,
        string? ReferenceNo);
}
