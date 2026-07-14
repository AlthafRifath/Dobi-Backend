using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Inspections
{
    public sealed record CreateInspectionIssueRequest(
        int InspectionIssueTypeId,
        string? Notes);
}
