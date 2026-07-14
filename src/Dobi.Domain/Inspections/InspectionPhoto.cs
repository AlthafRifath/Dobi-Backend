using Dobi.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Inspections
{
    public class InspectionPhoto : AuditableEntity
    {
        public int InspectionRecordId { get; set; }
        public InspectionRecord InspectionRecord { get; set; } = null!;

        public string PhotoUrl { get; set; } = string.Empty;
        public int UploadedByUserId { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
