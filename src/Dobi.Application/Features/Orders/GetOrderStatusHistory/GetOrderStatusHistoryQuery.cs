using Dobi.Contracts.Orders;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Orders.GetOrderStatusHistory
{
    public sealed record GetOrderStatusHistoryQuery(
        int OrderId) : IRequest<IReadOnlyCollection<OrderStatusHistoryResponse>>;
}
