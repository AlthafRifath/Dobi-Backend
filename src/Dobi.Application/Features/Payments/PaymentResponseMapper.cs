using Dobi.Application.Common;
using Dobi.Contracts.Payments;
using Dobi.Domain.Payments;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Payments
{
    internal static class PaymentResponseMapper
    {
        public static PaymentResponse Map(
            Payment payment,
            PaymentAmountSummary summary)
        {
            return new PaymentResponse(
                payment.Id,
                payment.OrderId,
                payment.Order.OrderNo,

                payment.Amount,

                payment.PaymentMethodId,
                LookupValueHelper.GetCode(payment.PaymentMethod),
                LookupValueHelper.GetName(payment.PaymentMethod),

                payment.PaymentStatusId,
                LookupValueHelper.GetCode(payment.PaymentStatus),
                LookupValueHelper.GetName(payment.PaymentStatus),

                payment.PaidAt,
                payment.ReceivedByUserId,

                payment.ReferenceNo,

                payment.ChequeNo,
                payment.ChequeBankName,
                payment.ChequeDate,
                payment.ChequeClearedAt,
                payment.ChequeRejectedAt,
                payment.ChequeRejectionReason,

                payment.CreatedAt,

                summary.OrderTotalAmount,
                summary.PaidAmount,
                summary.PendingClearanceAmount,
                summary.FailedAmount,
                summary.OutstandingAmount,

                summary.OrderPaymentStatusId,
                summary.OrderPaymentStatusCode,
                summary.OrderPaymentStatusName);
        }
    }
}
