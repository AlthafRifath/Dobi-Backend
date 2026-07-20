using Dobi.Application.Features.Orders.CancelOrder;
using Dobi.Application.Features.Orders.CreateOrder;
using Dobi.Application.Features.Orders.GetEligibleOutletReturnOrders;
using Dobi.Application.Features.Orders.GetEligiblePaymentOrders;
using Dobi.Application.Features.Orders.GetEligiblePlantProcessingOrders;
using Dobi.Application.Features.Orders.GetEligiblePlantTransferOrders;
using Dobi.Application.Features.Orders.GetEligibleRefundOrders;
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

    [HttpGet("eligible-for-plant-transfer")]
    public async Task<ActionResult<ApiResponse<PagedResponse<EligiblePlantTransferOrderResponse>>>> GetEligibleForPlantTransfer(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string? searchTerm = null,
    [FromQuery] int fromBranchId = 0,
    [FromQuery] int toPlantId = 0,
    [FromQuery] int? customerTypeId = null,
    [FromQuery] int[]? excludeOrderIds = null,
    CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(
            new GetEligiblePlantTransferOrdersQuery(
                pageNumber,
                pageSize,
                searchTerm,
                fromBranchId,
                toPlantId,
                customerTypeId,
                excludeOrderIds ?? Array.Empty<int>()),
            cancellationToken);

        return Ok(ApiResponse<PagedResponse<EligiblePlantTransferOrderResponse>>.Ok(
            response,
            "Eligible plant transfer orders loaded successfully."));
    }

    [HttpGet("eligible-for-outlet-return")]
    public async Task<ActionResult<ApiResponse<PagedResponse<EligibleOutletReturnOrderResponse>>>> GetEligibleForOutletReturn(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string? searchTerm = null,
    [FromQuery] int fromPlantId = 0,
    [FromQuery] int toBranchId = 0,
    [FromQuery] int[]? excludeOrderIds = null,
    CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(
            new GetEligibleOutletReturnOrdersQuery(
                pageNumber,
                pageSize,
                searchTerm,
                fromPlantId,
                toBranchId,
                excludeOrderIds ?? Array.Empty<int>()),
            cancellationToken);

        return Ok(ApiResponse<PagedResponse<EligibleOutletReturnOrderResponse>>.Ok(
            response,
            "Eligible outlet return orders loaded successfully."));
    }

    [HttpGet("eligible-for-plant-processing")]
    public async Task<ActionResult<ApiResponse<PagedResponse<EligiblePlantProcessingOrderResponse>>>> GetEligibleForPlantProcessing(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string? searchTerm = null,
    [FromQuery] int plantId = 0,
    [FromQuery] int? customerId = null,
    [FromQuery] int? branchId = null,
    [FromQuery] int? customerTypeId = null,
    [FromQuery] int[]? excludeOrderIds = null,
    CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(
            new GetEligiblePlantProcessingOrdersQuery(
                pageNumber,
                pageSize,
                searchTerm,
                plantId,
                customerId,
                branchId,
                customerTypeId,
                excludeOrderIds ?? Array.Empty<int>()),
            cancellationToken);

        return Ok(ApiResponse<PagedResponse<EligiblePlantProcessingOrderResponse>>.Ok(
            response,
            "Eligible plant processing orders loaded successfully."));
    }

    [HttpGet("eligible-for-payment")]
    public async Task<ActionResult<ApiResponse<PagedResponse<EligiblePaymentOrderResponse>>>> GetEligibleForPayment(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string? searchTerm = null,
    [FromQuery] int? branchId = null,
    [FromQuery] int? customerId = null,
    [FromQuery] int? customerTypeId = null,
    [FromQuery] int[]? excludeOrderIds = null,
    CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(
            new GetEligiblePaymentOrdersQuery(
                pageNumber,
                pageSize,
                searchTerm,
                branchId,
                customerId,
                customerTypeId,
                excludeOrderIds ?? Array.Empty<int>()),
            cancellationToken);

        return Ok(ApiResponse<PagedResponse<EligiblePaymentOrderResponse>>.Ok(
            response,
            "Eligible payment orders loaded successfully."));
    }

    [HttpGet("eligible-for-refund")]
    public async Task<ActionResult<ApiResponse<PagedResponse<EligibleRefundOrderResponse>>>> GetEligibleForRefund(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string? searchTerm = null,
    [FromQuery] int? customerId = null,
    [FromQuery] int? branchId = null,
    [FromQuery] int? customerTypeId = null,
    [FromQuery] int[]? excludeOrderIds = null,
    CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(
            new GetEligibleRefundOrdersQuery(
                pageNumber,
                pageSize,
                searchTerm,
                customerId,
                branchId,
                customerTypeId,
                excludeOrderIds ?? Array.Empty<int>()),
            cancellationToken);

        return Ok(ApiResponse<PagedResponse<EligibleRefundOrderResponse>>.Ok(
            response,
            "Eligible refund orders loaded successfully."));
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