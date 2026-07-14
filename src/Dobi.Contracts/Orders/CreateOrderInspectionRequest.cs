using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Orders
{
    public sealed record CreateOrderInspectionRequest(
        int? OrderItemTempIndex,
        bool CustomerAcknowledged,
        string? CustomerSignatureUrl,
        string? Notes,
        IReadOnlyCollection<int> InspectionIssueTypeIds,
        IReadOnlyCollection<string>? PhotoUrls);
}
