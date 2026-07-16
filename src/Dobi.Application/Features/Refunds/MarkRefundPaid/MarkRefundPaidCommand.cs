using Dobi.Contracts.Refunds;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Refunds.MarkRefundPaid
{
    public sealed record MarkRefundPaidCommand(
        int RefundId) : IRequest<RefundResponse>;
}
