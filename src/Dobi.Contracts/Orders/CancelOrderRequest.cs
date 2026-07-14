using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Orders
{
    public sealed record CancelOrderRequest(
        string CancellationReason,
        bool RequestedByCustomer = true);
}
