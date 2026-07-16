using Dobi.Contracts.Payments;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Payments.MarkChequeRejected
{
    public sealed record MarkChequeRejectedCommand(
        int PaymentId,
        string RejectionReason) : IRequest<PaymentResponse>;
}
