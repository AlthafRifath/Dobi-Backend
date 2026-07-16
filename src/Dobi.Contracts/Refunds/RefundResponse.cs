using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Refunds
{
    public sealed record RefundResponse(
        int RefundId,
        int OrderId,
        string OrderNo,
        int? PaymentId,
        decimal Amount,
        string Reason,
        int RefundStatusId,
        string RefundStatusCode,
        string RefundStatusName,
        int? ApprovedByUserId,
        DateTime? RefundedAt,
        int? RejectedByUserId,
        DateTime? RejectedAt,
        string? RejectionReason,
        DateTime CreatedAt);
}
