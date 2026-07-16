using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Reports
{
    public sealed record RefundReportResponse(
        int RefundId,
        int OrderId,
        string OrderNo,
        int? PaymentId,
        decimal Amount,
        string Reason,
        int RefundStatusId,
        string RefundStatusName,
        DateTime CreatedAt,
        DateTime? RefundedAt);
}
