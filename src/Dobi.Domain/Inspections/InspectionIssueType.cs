using Dobi.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Inspections
{
    public class InspectionIssueType : BaseEntity
    {
        public string IssueCode { get; set; } = string.Empty; // STAIN, TEAR, MISSING_BUTTONS
        public string IssueName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public ICollection<InspectionIssue> InspectionIssues { get; set; } = new List<InspectionIssue>();
    }
}
