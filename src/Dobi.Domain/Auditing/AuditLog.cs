using Dobi.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Auditing
{
    public class AuditLog : BaseEntity
    {
        public string EntityName { get; set; } = string.Empty;
        public int? EntityId { get; set; }

        public string Action { get; set; } = string.Empty;

        public string? OldValues { get; set; }
        public string? NewValues { get; set; }

        public int? PerformedByUserId { get; set; }
        public DateTime PerformedAt { get; set; }

        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
    }
}
