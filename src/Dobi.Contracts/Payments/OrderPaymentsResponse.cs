using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Payments
{
    public sealed record OrderPaymentsResponse(
        int OrderId,
        string OrderNo,
        decimal OrderTotalAmount,
        decimal PaidAmount,
        decimal PendingClearanceAmount,
        decimal FailedAmount,
        decimal OutstandingAmount,
        int OrderPaymentStatusId,
        string OrderPaymentStatusCode,
        string OrderPaymentStatusName,
        IReadOnlyCollection<PaymentResponse> Payments);
}
