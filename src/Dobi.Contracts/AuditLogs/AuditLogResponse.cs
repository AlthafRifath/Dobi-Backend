using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.AuditLogs
{
    public sealed record AuditLogResponse(
        int AuditLogId,
        string EntityName,
        int? EntityId,
        string Action,
        string? OldValues,
        string? NewValues,
        int? PerformedByUserId,
        DateTime PerformedAt,
        string? IpAddress,
        string? UserAgent);
}
