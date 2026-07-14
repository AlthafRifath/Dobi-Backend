using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Orders
{
    public sealed record OrderStatusHistoryResponse(
        int StatusHistoryId,
        int OrderId,
        int OrderStatusId,
        string StatusName,
        string? Remarks,
        int ChangedByUserId,
        DateTime ChangedAt);
}
