using Dobi.Contracts.Orders;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Orders.GetOrderById
{
    public sealed record GetOrderByIdQuery(
        int OrderId) : IRequest<OrderResponse>;
}
