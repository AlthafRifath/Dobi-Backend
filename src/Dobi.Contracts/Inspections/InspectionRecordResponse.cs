using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Inspections
{
    public sealed record InspectionRecordResponse(
        int InspectionRecordId,
        int OrderId,
        int? OrderItemId,
        bool CustomerAcknowledged,
        string? CustomerSignatureUrl,
        string? Notes,
        int InspectedByUserId,
        DateTime InspectedAt,
        IReadOnlyCollection<InspectionIssueResponse> Issues,
        IReadOnlyCollection<InspectionPhotoResponse> Photos);
}
