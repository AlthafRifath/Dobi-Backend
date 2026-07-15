using Dobi.Application.Features.Transfers.AcknowledgeTransfer;
using Dobi.Application.Features.Transfers.CreateTransferBatch;
using Dobi.Application.Features.Transfers.GetTransferById;
using Dobi.Application.Features.Transfers.GetTransfers;
using Dobi.Contracts.Common;
using Dobi.Contracts.Transfers;
using Dobi.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dobi.Api.Controllers;

[ApiController]
[Route("api/transfers")]
[Authorize(Roles = TransferReadRoles)]
public sealed class TransfersController : ControllerBase
{
    private const string TransferReadRoles =
        RoleCodes.Admin + "," +
        RoleCodes.OutletStaff + "," +
        RoleCodes.PlantSupervisor + "," +
        RoleCodes.Driver + "," +
        RoleCodes.Manager + "," +
        RoleCodes.OperationsDirector;

    private const string TransferWriteRoles =
        RoleCodes.Admin + "," +
        RoleCodes.OutletStaff + "," +
        RoleCodes.PlantSupervisor + "," +
        RoleCodes.Manager;

    private const string TransferAcknowledgeRoles =
        RoleCodes.Admin + "," +
        RoleCodes.OutletStaff + "," +
        RoleCodes.PlantSupervisor + "," +
        RoleCodes.Driver + "," +
        RoleCodes.Manager;

    private readonly IMediator _mediator;

    public TransfersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<TransferBatchResponse>>>> GetTransfers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] int? transferTypeId = null,
        [FromQuery] int? transferStatusId = null,
        [FromQuery] int? driverUserId = null,
        [FromQuery] DateOnly? fromDate = null,
        [FromQuery] DateOnly? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(
            new GetTransfersQuery(
                pageNumber,
                pageSize,
                searchTerm,
                transferTypeId,
                transferStatusId,
                driverUserId,
                fromDate,
                toDate),
            cancellationToken);

        return Ok(ApiResponse<PagedResponse<TransferBatchResponse>>.Ok(
            response,
            "Transfers loaded successfully."));
    }

    [HttpGet("{transferBatchId:int}")]
    public async Task<ActionResult<ApiResponse<TransferBatchResponse>>> GetTransferById(
        [FromRoute] int transferBatchId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetTransferByIdQuery(transferBatchId),
            cancellationToken);

        return Ok(ApiResponse<TransferBatchResponse>.Ok(
            response,
            "Transfer loaded successfully."));
    }

    [Authorize(Roles = TransferWriteRoles)]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<TransferBatchResponse>>> CreateTransfer(
        [FromBody] CreateTransferBatchRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new CreateTransferBatchCommand(
                request.TransferTypeId,
                request.FromBranchId,
                request.FromPlantId,
                request.ToBranchId,
                request.ToPlantId,
                request.DriverUserId,
                request.Items,
                request.Remarks),
            cancellationToken);

        return Ok(ApiResponse<TransferBatchResponse>.Ok(
            response,
            "Transfer created successfully."));
    }

    [Authorize(Roles = TransferAcknowledgeRoles)]
    [HttpPost("{transferBatchId:int}/acknowledgements")]
    public async Task<ActionResult<ApiResponse<TransferBatchResponse>>> AcknowledgeTransfer(
        [FromRoute] int transferBatchId,
        [FromBody] AcknowledgeTransferRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new AcknowledgeTransferCommand(
                transferBatchId,
                request.AcknowledgementTypeId,
                request.SignatureUrl,
                request.Remarks),
            cancellationToken);

        return Ok(ApiResponse<TransferBatchResponse>.Ok(
            response,
            "Transfer acknowledgement recorded successfully."));
    }
}