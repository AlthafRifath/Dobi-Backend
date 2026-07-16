using Dobi.Application.Common;
using Dobi.Contracts.Refunds;
using Dobi.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Refunds
{
    internal static class RefundResponseMapper
    {
        public static RefundResponse Map(Refund refund)
        {
            return new RefundResponse(
                refund.Id,
                refund.OrderId,
                refund.Order.OrderNo,
                refund.PaymentId,
                refund.Amount,
                refund.Reason,
                refund.RefundStatusId,
                LookupValueHelper.GetCode(refund.RefundStatus),
                LookupValueHelper.GetName(refund.RefundStatus),
                refund.ApprovedByUserId,
                refund.RefundedAt,
                refund.RejectedByUserId,
                refund.RejectedAt,
                refund.RejectionReason,
                refund.CreatedAt);
        }
    }
}
