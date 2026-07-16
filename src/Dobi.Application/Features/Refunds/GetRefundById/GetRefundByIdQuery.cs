using Dobi.Contracts.Refunds;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Refunds.GetRefundById
{
    public sealed record GetRefundByIdQuery(
        int RefundId) : IRequest<RefundResponse>;
}
