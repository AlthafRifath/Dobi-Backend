using Dobi.Contracts.Refunds;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Refunds.RejectRefund
{
    public sealed record RejectRefundCommand(
        int RefundId,
        string RejectionReason) : IRequest<RefundResponse>;
}
