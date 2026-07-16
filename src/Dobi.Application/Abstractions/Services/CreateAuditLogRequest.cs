using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Abstractions.Services
{
    public sealed record CreateAuditLogRequest(
        string EntityName,
        int? EntityId,
        string Action,
        string? OldValues,
        string? NewValues,
        int? PerformedByUserId,
        string? IpAddress,
        string? UserAgent);
}
