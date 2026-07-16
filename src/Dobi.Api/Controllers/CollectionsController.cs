using Dobi.Application.Features.Collections.CompleteOrderCollection;
using Dobi.Application.Features.Collections.GetCollectionById;
using Dobi.Application.Features.Collections.GetCollectionByOrder;
using Dobi.Application.Features.Collections.GetCollections;
using Dobi.Application.Features.Collections.MarkOrderReadyForCollection;
using Dobi.Contracts.Collections;
using Dobi.Contracts.Common;
using Dobi.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dobi.Api.Controllers;

[ApiController]
[Route("api/collections")]
[Authorize(Roles = CollectionReadRoles)]
public sealed class CollectionsController : ControllerBase
{
    private const string CollectionReadRoles =
        RoleCodes.Admin + "," +
        RoleCodes.OutletStaff + "," +
        RoleCodes.PlantSupervisor + "," +
        RoleCodes.Driver + "," +
        RoleCodes.Manager + "," +
        RoleCodes.OperationsDirector;

    private const string CollectionWriteRoles =
        RoleCodes.Admin + "," +
        RoleCodes.OutletStaff + "," +
        RoleCodes.PlantSupervisor + "," +
        RoleCodes.Driver + "," +
        RoleCodes.Manager;

    private readonly IMediator _mediator;

    public CollectionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<OrderCollectionResponse>>>> GetCollections(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] int? collectionModeId = null,
        [FromQuery] DateOnly? fromDate = null,
        [FromQuery] DateOnly? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(
            new GetCollectionsQuery(
                pageNumber,
                pageSize,
                searchTerm,
                collectionModeId,
                fromDate,
                toDate),
            cancellationToken);

        return Ok(ApiResponse<PagedResponse<OrderCollectionResponse>>.Ok(
            response,
            "Collections loaded successfully."));
    }

    [HttpGet("{orderCollectionId:int}")]
    public async Task<ActionResult<ApiResponse<OrderCollectionResponse>>> GetCollectionById(
        [FromRoute] int orderCollectionId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetCollectionByIdQuery(orderCollectionId),
            cancellationToken);

        return Ok(ApiResponse<OrderCollectionResponse>.Ok(
            response,
            "Collection loaded successfully."));
    }

    [HttpGet("by-order/{orderId:int}")]
    public async Task<ActionResult<ApiResponse<OrderCollectionResponse>>> GetCollectionByOrder(
        [FromRoute] int orderId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetCollectionByOrderQuery(orderId),
            cancellationToken);

        return Ok(ApiResponse<OrderCollectionResponse>.Ok(
            response,
            "Collection loaded successfully."));
    }

    [Authorize(Roles = CollectionWriteRoles)]
    [HttpPost("orders/{orderId:int}/mark-ready")]
    public async Task<ActionResult<ApiResponse<OrderCollectionResponse>>> MarkOrderReady(
        [FromRoute] int orderId,
        [FromBody] MarkOrderReadyForCollectionRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new MarkOrderReadyForCollectionCommand(
                orderId,
                request.Remarks),
            cancellationToken);

        return Ok(ApiResponse<OrderCollectionResponse>.Ok(
            response,
            "Order marked ready for collection or delivery successfully."));
    }

    [Authorize(Roles = CollectionWriteRoles)]
    [HttpPost("orders/{orderId:int}/complete")]
    public async Task<ActionResult<ApiResponse<OrderCollectionResponse>>> CompleteCollection(
        [FromRoute] int orderId,
        [FromBody] CompleteOrderCollectionRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new CompleteOrderCollectionCommand(
                orderId,
                request.IsCollectedByCustomer,
                request.CollectorName,
                request.CollectorMobileNo,
                request.ReceiptVerified,
                request.MobileNoVerified,
                request.ReceiptImageUrl,
                request.CustomerSignatureUrl,
                request.Remarks),
            cancellationToken);

        return Ok(ApiResponse<OrderCollectionResponse>.Ok(
            response,
            "Order collection or delivery completed successfully."));
    }
}