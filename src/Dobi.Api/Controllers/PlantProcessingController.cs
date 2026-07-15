using Dobi.Application.Features.PlantProcessing.GetPlantProcessing;
using Dobi.Application.Features.PlantProcessing.GetPlantProcessingById;
using Dobi.Application.Features.PlantProcessing.GetPlantProcessingByOrder;
using Dobi.Application.Features.PlantProcessing.MarkReadyForOutletReturn;
using Dobi.Application.Features.PlantProcessing.RecordQc;
using Dobi.Application.Features.PlantProcessing.StartPlantProcessing;
using Dobi.Application.Features.PlantProcessing.UpdatePlantProcessingStage;
using Dobi.Contracts.Common;
using Dobi.Contracts.PlantProcessing;
using Dobi.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dobi.Api.Controllers;

[ApiController]
[Route("api/plant-processing")]
[Authorize(Roles = PlantProcessingReadRoles)]
public sealed class PlantProcessingController : ControllerBase
{
    private const string PlantProcessingReadRoles =
        RoleCodes.Admin + "," +
        RoleCodes.OutletStaff + "," +
        RoleCodes.PlantSupervisor + "," +
        RoleCodes.Manager + "," +
        RoleCodes.OperationsDirector;

    private const string PlantProcessingWriteRoles =
        RoleCodes.Admin + "," +
        RoleCodes.PlantSupervisor + "," +
        RoleCodes.Manager;

    private readonly IMediator _mediator;

    public PlantProcessingController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<PlantProcessingResponse>>>> GetPlantProcessing(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] int? plantId = null,
        [FromQuery] int? orderId = null,
        [FromQuery] DateOnly? fromDate = null,
        [FromQuery] DateOnly? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(
            new GetPlantProcessingQuery(
                pageNumber,
                pageSize,
                plantId,
                orderId,
                fromDate,
                toDate),
            cancellationToken);

        return Ok(ApiResponse<PagedResponse<PlantProcessingResponse>>.Ok(
            response,
            "Plant processing records loaded successfully."));
    }

    [HttpGet("{plantProcessingId:int}")]
    public async Task<ActionResult<ApiResponse<PlantProcessingResponse>>> GetPlantProcessingById(
        [FromRoute] int plantProcessingId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetPlantProcessingByIdQuery(plantProcessingId),
            cancellationToken);

        return Ok(ApiResponse<PlantProcessingResponse>.Ok(
            response,
            "Plant processing record loaded successfully."));
    }

    [HttpGet("by-order/{orderId:int}")]
    public async Task<ActionResult<ApiResponse<PlantProcessingResponse>>> GetPlantProcessingByOrder(
        [FromRoute] int orderId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetPlantProcessingByOrderQuery(orderId),
            cancellationToken);

        return Ok(ApiResponse<PlantProcessingResponse>.Ok(
            response,
            "Plant processing record loaded successfully."));
    }

    [Authorize(Roles = PlantProcessingWriteRoles)]
    [HttpPost("orders/{orderId:int}/start")]
    public async Task<ActionResult<ApiResponse<PlantProcessingResponse>>> StartPlantProcessing(
        [FromRoute] int orderId,
        [FromBody] StartPlantProcessingRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new StartPlantProcessingCommand(
                orderId,
                request.PlantId,
                request.PlantRemarks),
            cancellationToken);

        return Ok(ApiResponse<PlantProcessingResponse>.Ok(
            response,
            "Plant processing started successfully."));
    }

    [Authorize(Roles = PlantProcessingWriteRoles)]
    [HttpPost("{plantProcessingId:int}/stage-updates")]
    public async Task<ActionResult<ApiResponse<PlantProcessingResponse>>> UpdateStage(
        [FromRoute] int plantProcessingId,
        [FromBody] UpdatePlantProcessingStageRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new UpdatePlantProcessingStageCommand(
                plantProcessingId,
                request.ProcessingStageId,
                request.ProcessingStageStatusId,
                request.Remarks),
            cancellationToken);

        return Ok(ApiResponse<PlantProcessingResponse>.Ok(
            response,
            "Plant processing stage updated successfully."));
    }

    [Authorize(Roles = PlantProcessingWriteRoles)]
    [HttpPost("{plantProcessingId:int}/qc")]
    public async Task<ActionResult<ApiResponse<PlantProcessingResponse>>> RecordQc(
        [FromRoute] int plantProcessingId,
        [FromBody] RecordQcRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new RecordQcCommand(
                plantProcessingId,
                request.OrderItemId,
                request.QcStatusId,
                request.IssueDescription,
                request.ActionTaken,
                request.LabourChargeAmount),
            cancellationToken);

        return Ok(ApiResponse<PlantProcessingResponse>.Ok(
            response,
            "QC result recorded successfully."));
    }

    [Authorize(Roles = PlantProcessingWriteRoles)]
    [HttpPost("{plantProcessingId:int}/ready-for-outlet-return")]
    public async Task<ActionResult<ApiResponse<PlantProcessingResponse>>> MarkReadyForOutletReturn(
        [FromRoute] int plantProcessingId,
        [FromBody] MarkReadyForOutletReturnRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new MarkReadyForOutletReturnCommand(
                plantProcessingId,
                request.ReadyDate,
                request.PlantRemarks),
            cancellationToken);

        return Ok(ApiResponse<PlantProcessingResponse>.Ok(
            response,
            "B2C order marked ready for outlet return successfully."));
    }
}