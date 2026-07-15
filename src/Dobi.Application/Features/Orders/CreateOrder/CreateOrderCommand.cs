using Dobi.Contracts.Orders;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Orders.CreateOrder
{
    public sealed record CreateOrderCommand(
        int CustomerId,
        int BranchId,
        DateOnly? ExpectedReturnDate,
        bool IsExpress,
        IReadOnlyCollection<CreateOrderItemRequest> Items,
        IReadOnlyCollection<CreateOrderInspectionRequest>? Inspections) : IRequest<OrderResponse>;
}
