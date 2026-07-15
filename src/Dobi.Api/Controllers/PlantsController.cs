using Dobi.Application.Features.Plants.CreatePlant;
using Dobi.Application.Features.Plants.GetPlantById;
using Dobi.Application.Features.Plants.GetPlants;
using Dobi.Application.Features.Plants.UpdatePlant;
using Dobi.Application.Features.Plants.UpdatePlantStatus;
using Dobi.Contracts.Common;
using Dobi.Contracts.Plants;
using Dobi.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dobi.Api.Controllers;

[ApiController]
[Route("api/plants")]
[Authorize]
public sealed class PlantsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PlantsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<PlantResponse>>>> GetPlants(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] bool? isActive = null,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(
            new GetPlantsQuery(
                pageNumber,
                pageSize,
                searchTerm,
                isActive),
            cancellationToken);

        return Ok(ApiResponse<PagedResponse<PlantResponse>>.Ok(
            response,
            "Plants loaded successfully."));
    }

    [HttpGet("{plantId:int}")]
    public async Task<ActionResult<ApiResponse<PlantResponse>>> GetPlantById(
        [FromRoute] int plantId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetPlantByIdQuery(plantId),
            cancellationToken);

        return Ok(ApiResponse<PlantResponse>.Ok(
            response,
            "Plant loaded successfully."));
    }

    [Authorize(Roles = RoleCodes.Admin)]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<PlantResponse>>> CreatePlant(
        [FromBody] PlantRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new CreatePlantCommand(
                request.PlantName,
                request.Address,
                request.OperatingHours,
                request.IsActive),
            cancellationToken);

        return Ok(ApiResponse<PlantResponse>.Ok(
            response,
            "Plant created successfully."));
    }

    [Authorize(Roles = RoleCodes.Admin)]
    [HttpPut("{plantId:int}")]
    public async Task<ActionResult<ApiResponse<PlantResponse>>> UpdatePlant(
        [FromRoute] int plantId,
        [FromBody] PlantRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new UpdatePlantCommand(
                plantId,
                request.PlantName,
                request.Address,
                request.OperatingHours,
                request.IsActive),
            cancellationToken);

        return Ok(ApiResponse<PlantResponse>.Ok(
            response,
            "Plant updated successfully."));
    }

    [Authorize(Roles = RoleCodes.Admin)]
    [HttpPatch("{plantId:int}/status")]
    public async Task<ActionResult<ApiResponse<PlantResponse>>> UpdatePlantStatus(
        [FromRoute] int plantId,
        [FromBody] UpdateStatusRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new UpdatePlantStatusCommand(
                plantId,
                request.IsActive),
            cancellationToken);

        return Ok(ApiResponse<PlantResponse>.Ok(
            response,
            request.IsActive
                ? "Plant activated successfully."
                : "Plant deactivated successfully."));
    }
}