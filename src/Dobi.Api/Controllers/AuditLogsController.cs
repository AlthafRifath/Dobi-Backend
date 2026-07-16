using Dobi.Application.Features.AuditLogs.GetAuditLogById;
using Dobi.Application.Features.AuditLogs.GetAuditLogs;
using Dobi.Contracts.AuditLogs;
using Dobi.Contracts.Common;
using Dobi.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dobi.Api.Controllers;

[ApiController]
[Route("api/audit-logs")]
[Authorize(Roles = AuditLogReadRoles)]
public sealed class AuditLogsController : ControllerBase
{
    private const string AuditLogReadRoles =
        RoleCodes.Admin + "," +
        RoleCodes.Manager + "," +
        RoleCodes.OperationsDirector;

    private readonly IMediator _mediator;

    public AuditLogsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<AuditLogResponse>>>> GetAuditLogs(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] string? entityName = null,
        [FromQuery] int? entityId = null,
        [FromQuery] string? action = null,
        [FromQuery] int? performedByUserId = null,
        [FromQuery] DateOnly? fromDate = null,
        [FromQuery] DateOnly? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(
            new GetAuditLogsQuery(
                pageNumber,
                pageSize,
                searchTerm,
                entityName,
                entityId,
                action,
                performedByUserId,
                fromDate,
                toDate),
            cancellationToken);

        return Ok(ApiResponse<PagedResponse<AuditLogResponse>>.Ok(
            response,
            "Audit logs loaded successfully."));
    }

    [HttpGet("{auditLogId:int}")]
    public async Task<ActionResult<ApiResponse<AuditLogResponse>>> GetAuditLogById(
        [FromRoute] int auditLogId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetAuditLogByIdQuery(auditLogId),
            cancellationToken);

        return Ok(ApiResponse<AuditLogResponse>.Ok(
            response,
            "Audit log loaded successfully."));
    }
}