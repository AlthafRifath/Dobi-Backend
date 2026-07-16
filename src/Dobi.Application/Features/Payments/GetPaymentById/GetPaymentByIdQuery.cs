using Dobi.Contracts.Payments;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Payments.GetPaymentById
{
    public sealed record GetPaymentByIdQuery(
        int PaymentId) : IRequest<PaymentResponse>;
}
