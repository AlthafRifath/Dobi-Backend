using Dobi.Contracts.AuditLogs;
using Dobi.Domain.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.AuditLogs
{
    internal static class AuditLogResponseMapper
    {
        public static AuditLogResponse Map(AuditLog auditLog)
        {
            return new AuditLogResponse(
                auditLog.Id,
                auditLog.EntityName,
                auditLog.EntityId,
                auditLog.Action,
                auditLog.OldValues,
                auditLog.NewValues,
                auditLog.PerformedByUserId,
                auditLog.PerformedAt,
                auditLog.IpAddress,
                auditLog.UserAgent);
        }
    }
}
