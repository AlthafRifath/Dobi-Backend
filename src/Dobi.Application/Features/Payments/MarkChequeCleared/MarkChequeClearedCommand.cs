using Dobi.Contracts.Payments;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Payments.MarkChequeCleared
{
    public sealed record MarkChequeClearedCommand(
        int PaymentId) : IRequest<PaymentResponse>;
}
