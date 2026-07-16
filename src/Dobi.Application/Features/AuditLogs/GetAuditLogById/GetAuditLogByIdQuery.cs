using Dobi.Contracts.AuditLogs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.AuditLogs.GetAuditLogById
{
    public sealed record GetAuditLogByIdQuery(
        int AuditLogId) : IRequest<AuditLogResponse>;
}
