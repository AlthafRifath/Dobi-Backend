using Dobi.Application.Features.Services.CreateService;
using Dobi.Application.Features.Services.GetServiceById;
using Dobi.Application.Features.Services.GetServices;
using Dobi.Application.Features.Services.UpdateService;
using Dobi.Application.Features.Services.UpdateServiceStatus;
using Dobi.Contracts.Common;
using Dobi.Contracts.Services;
using Dobi.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dobi.Api.Controllers;

[ApiController]
[Route("api/services")]
[Authorize]
public sealed class ServicesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ServicesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<ServiceResponse>>>> GetServices(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] bool? isActive = null,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(
            new GetServicesQuery(pageNumber, pageSize, searchTerm, isActive),
            cancellationToken);

        return Ok(ApiResponse<PagedResponse<ServiceResponse>>.Ok(response, "Services loaded successfully."));
    }

    [HttpGet("{serviceId:int}")]
    public async Task<ActionResult<ApiResponse<ServiceResponse>>> GetServiceById(
        [FromRoute] int serviceId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetServiceByIdQuery(serviceId),
            cancellationToken);

        return Ok(ApiResponse<ServiceResponse>.Ok(response, "Service loaded successfully."));
    }

    [Authorize(Roles = RoleCodes.Admin)]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<ServiceResponse>>> CreateService(
        [FromBody] CreateServiceRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new CreateServiceCommand(
                request.ServiceCode,
                request.ServiceName,
                request.Description,
                request.IsExpressEligible,
                request.IsActive),
            cancellationToken);

        return Ok(ApiResponse<ServiceResponse>.Ok(response, "Service created successfully."));
    }

    [Authorize(Roles = RoleCodes.Admin)]
    [HttpPut("{serviceId:int}")]
    public async Task<ActionResult<ApiResponse<ServiceResponse>>> UpdateService(
        [FromRoute] int serviceId,
        [FromBody] CreateServiceRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new UpdateServiceCommand(
                serviceId,
                request.ServiceCode,
                request.ServiceName,
                request.Description,
                request.IsExpressEligible,
                request.IsActive),
            cancellationToken);

        return Ok(ApiResponse<ServiceResponse>.Ok(response, "Service updated successfully."));
    }

    [Authorize(Roles = RoleCodes.Admin)]
    [HttpPatch("{serviceId:int}/status")]
    public async Task<ActionResult<ApiResponse<ServiceResponse>>> UpdateServiceStatus(
        [FromRoute] int serviceId,
        [FromBody] UpdateStatusRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new UpdateServiceStatusCommand(serviceId, request.IsActive),
            cancellationToken);

        return Ok(ApiResponse<ServiceResponse>.Ok(
            response,
            request.IsActive ? "Service activated successfully." : "Service deactivated successfully."));
    }
}