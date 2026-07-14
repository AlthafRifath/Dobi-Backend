using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Orders
{
    public sealed record UpdateOrderStatusRequest(
        int OrderStatusId,
        string? Remarks);
}
