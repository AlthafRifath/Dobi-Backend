using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Orders
{
    public sealed record CreateOrderRequest(
        int CustomerId,
        int BranchId,
        DateOnly? ExpectedReturnDate,
        bool IsExpress,
        IReadOnlyCollection<CreateOrderItemRequest> Items,
        IReadOnlyCollection<CreateOrderInspectionRequest>? Inspections);
}
