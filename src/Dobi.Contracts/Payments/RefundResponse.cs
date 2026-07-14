using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Payments
{
    public sealed record RefundResponse(
         int RefundId,
         int OrderId,
         string OrderNo,
         int? PaymentId,
         decimal Amount,
         string Reason,
         int RefundStatusId,
         string RefundStatusName,
         int? ApprovedByUserId,
         DateTime? RefundedAt);
}
