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
        string PaymentMethodCode,
        string PaymentMethodName,

        int PaymentStatusId,
        string PaymentStatusCode,
        string PaymentStatusName,

        DateTime? PaidAt,
        int? ReceivedByUserId,

        string? ReferenceNo,

        string? ChequeNo,
        string? ChequeBankName,
        DateOnly? ChequeDate,
        DateTime? ChequeClearedAt,
        DateTime? ChequeRejectedAt,
        string? ChequeRejectionReason,

        DateTime CreatedAt,

        decimal OrderTotalAmount,
        decimal PaidAmount,
        decimal PendingClearanceAmount,
        decimal FailedAmount,
        decimal OutstandingAmount,

        int OrderPaymentStatusId,
        string OrderPaymentStatusCode,
        string OrderPaymentStatusName);
}
