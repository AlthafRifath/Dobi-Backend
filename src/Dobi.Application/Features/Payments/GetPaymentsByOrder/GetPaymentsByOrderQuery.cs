using Dobi.Contracts.Payments;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Payments.GetPaymentsByOrder
{
    public sealed record GetPaymentsByOrderQuery(
        int OrderId) : IRequest<OrderPaymentsResponse>;
}
