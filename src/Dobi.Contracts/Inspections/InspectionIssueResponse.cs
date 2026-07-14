using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Inspections
{
    public sealed record InspectionIssueResponse(
        int InspectionIssueId,
        int InspectionIssueTypeId,
        string IssueName,
        string? Notes);
}
