using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Inspections
{
    public sealed record CreateInspectionRecordRequest(
        int OrderId,
        int? OrderItemId,
        bool CustomerAcknowledged,
        string? CustomerSignatureUrl,
        string? Notes,
        IReadOnlyCollection<CreateInspectionIssueRequest> Issues,
        IReadOnlyCollection<string> PhotoUrls);
}
