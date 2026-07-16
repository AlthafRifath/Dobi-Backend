using Dobi.Contracts.AuditLogs;
using Dobi.Contracts.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.AuditLogs.GetAuditLogs
{
    public sealed record GetAuditLogsQuery(
        int PageNumber,
        int PageSize,
        string? SearchTerm,
        string? EntityName,
        int? EntityId,
        string? Action,
        int? PerformedByUserId,
        DateOnly? FromDate,
        DateOnly? ToDate) : IRequest<PagedResponse<AuditLogResponse>>;
}
