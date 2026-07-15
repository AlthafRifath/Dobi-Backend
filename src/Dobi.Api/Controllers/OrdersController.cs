using Dobi.Application.Features.Orders.CancelOrder;
using Dobi.Application.Features.Orders.CreateOrder;
using Dobi.Application.Features.Orders.GetOrderById;
using Dobi.Application.Features.Orders.GetOrders;
using Dobi.Application.Features.Orders.GetOrderStatusHistory;
using Dobi.Contracts.Common;
using Dobi.Contracts.Orders;
using Dobi.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dobi.Api.Controllers;

[ApiController]
[Route("api/orders")]
[Authorize(Roles = OrderReadRoles)]
public sealed class OrdersController : ControllerBase
{
    private const string OrderReadRoles =
        RoleCodes.Admin + "," +
        RoleCodes.OutletStaff + "," +
        RoleCodes.PlantSupervisor + "," +
        RoleCodes.Driver + "," +
        RoleCodes.Manager + "," +
        RoleCodes.OperationsDirector;

    private const string OrderWriteRoles =
        RoleCodes.Admin + "," +
        RoleCodes.OutletStaff + "," +
        RoleCodes.Manager;

    private const string OrderCancelRoles =
        RoleCodes.Admin + "," +
        RoleCodes.OutletStaff + "," +
        RoleCodes.Manager;

    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<OrderResponse>>>> GetOrders(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] int? branchId = null,
        [FromQuery] int? customerId = null,
        [FromQuery] int? statusId = null,
        [FromQuery] int? paymentStatusId = null,
        [FromQuery] DateOnly? fromDate = null,
        [FromQuery] DateOnly? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(
            new GetOrdersQuery(
                pageNumber,
                pageSize,
                searchTerm,
                branchId,
                customerId,
                statusId,
                paymentStatusId,
                fromDate,
                toDate),
            cancellationToken);

        return Ok(ApiResponse<PagedResponse<OrderResponse>>.Ok(
            response,
            "Orders loaded successfully."));
    }

    [HttpGet("{orderId:int}")]
    public async Task<ActionResult<ApiResponse<OrderResponse>>> GetOrderById(
        [FromRoute] int orderId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetOrderByIdQuery(orderId),
            cancellationToken);

        return Ok(ApiResponse<OrderResponse>.Ok(
            response,
            "Order loaded successfully."));
    }

    [HttpGet("{orderId:int}/status-history")]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<OrderStatusHistoryResponse>>>> GetOrderStatusHistory(
        [FromRoute] int orderId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetOrderStatusHistoryQuery(orderId),
            cancellationToken);

        return Ok(ApiResponse<IReadOnlyCollection<OrderStatusHistoryResponse>>.Ok(
            response,
            "Order status history loaded successfully."));
    }

    [Authorize(Roles = OrderWriteRoles)]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<OrderResponse>>> CreateOrder(
        [FromBody] CreateOrderRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new CreateOrderCommand(
                request.CustomerId,
                request.BranchId,
                request.ExpectedReturnDate,
                request.IsExpress,
                request.Items,
                request.Inspections),
            cancellationToken);

        return Ok(ApiResponse<OrderResponse>.Ok(
            response,
            "Order created successfully."));
    }

    [Authorize(Roles = OrderCancelRoles)]
    [HttpPost("{orderId:int}/cancel")]
    public async Task<ActionResult<ApiResponse<OrderResponse>>> CancelOrder(
        [FromRoute] int orderId,
        [FromBody] CancelOrderRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new CancelOrderCommand(
                orderId,
                request.CancellationReason,
                request.RequestedByCustomer),
            cancellationToken);

        return Ok(ApiResponse<OrderResponse>.Ok(
            response,
            "Order cancelled successfully."));
    }
}