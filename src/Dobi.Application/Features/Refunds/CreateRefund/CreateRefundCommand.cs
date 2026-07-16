using Dobi.Contracts.Refunds;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Refunds.CreateRefund
{
    public sealed record CreateRefundCommand(
        int OrderId,
        int? PaymentId,
        decimal Amount,
        string Reason) : IRequest<RefundResponse>;
}
