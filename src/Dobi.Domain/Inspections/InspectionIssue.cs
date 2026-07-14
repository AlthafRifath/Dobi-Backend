using Dobi.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Inspections
{
    public class InspectionIssue : BaseEntity
    {
        public int InspectionRecordId { get; set; }
        public InspectionRecord InspectionRecord { get; set; } = null!;

        public int InspectionIssueTypeId { get; set; }
        public InspectionIssueType InspectionIssueType { get; set; } = null!;

        public string? Notes { get; set; }
    }
}
