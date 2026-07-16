using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Payments
{
    internal sealed record PaymentAmountSummary(
        decimal OrderTotalAmount,
        decimal PaidAmount,
        decimal PendingClearanceAmount,
        decimal FailedAmount,
        decimal OutstandingAmount,
        int OrderPaymentStatusId,
        string OrderPaymentStatusCode,
        string OrderPaymentStatusName);
}
