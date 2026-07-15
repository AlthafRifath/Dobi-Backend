using Dobi.Contracts.Orders;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Orders.CancelOrder
{
    public sealed record CancelOrderCommand(
        int OrderId,
        string CancellationReason,
        bool RequestedByCustomer) : IRequest<OrderResponse>;
}
