using Dobi.Contracts.Refunds;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Refunds.ApproveRefund
{
    public sealed record ApproveRefundCommand(
        int RefundId) : IRequest<RefundResponse>;
}
