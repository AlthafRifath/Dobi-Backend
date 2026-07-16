using Dobi.Application.Features.Payments.GetPaymentById;
using Dobi.Application.Features.Payments.GetPayments;
using Dobi.Application.Features.Payments.GetPaymentsByOrder;
using Dobi.Application.Features.Payments.MarkChequeCleared;
using Dobi.Application.Features.Payments.MarkChequeRejected;
using Dobi.Application.Features.Payments.RecordPayment;
using Dobi.Contracts.Common;
using Dobi.Contracts.Payments;
using Dobi.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dobi.Api.Controllers;

[ApiController]
[Route("api/payments")]
[Authorize(Roles = PaymentReadRoles)]
public sealed class PaymentsController : ControllerBase
{
    private const string PaymentReadRoles =
        RoleCodes.Admin + "," +
        RoleCodes.OutletStaff + "," +
        RoleCodes.Manager + "," +
        RoleCodes.OperationsDirector;

    private const string PaymentWriteRoles =
        RoleCodes.Admin + "," +
        RoleCodes.OutletStaff + "," +
        RoleCodes.Manager;

    private readonly IMediator _mediator;

    public PaymentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<PaymentResponse>>>> GetPayments(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string? searchTerm = null,
    [FromQuery] int? orderId = null,
    [FromQuery] int? paymentMethodId = null,
    [FromQuery] int? paymentStatusId = null,
    [FromQuery] DateOnly? fromDate = null,
    [FromQuery] DateOnly? toDate = null,
    CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(
            new GetPaymentsQuery(
                pageNumber,
                pageSize,
                searchTerm,
                orderId,
                paymentMethodId,
                paymentStatusId,
                fromDate,
                toDate),
            cancellationToken);

        return Ok(ApiResponse<PagedResponse<PaymentResponse>>.Ok(
            response,
            "Payments loaded successfully."));
    }

    [HttpGet("{paymentId:int}")]
    public async Task<ActionResult<ApiResponse<PaymentResponse>>> GetPaymentById(
    [FromRoute] int paymentId,
    CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetPaymentByIdQuery(paymentId),
            cancellationToken);

        return Ok(ApiResponse<PaymentResponse>.Ok(
            response,
            "Payment loaded successfully."));
    }

    [HttpGet("by-order/{orderId:int}")]
    public async Task<ActionResult<ApiResponse<OrderPaymentsResponse>>> GetPaymentsByOrder(
        [FromRoute] int orderId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetPaymentsByOrderQuery(orderId),
            cancellationToken);

        return Ok(ApiResponse<OrderPaymentsResponse>.Ok(
            response,
            "Order payments loaded successfully."));
    }

    [Authorize(Roles = PaymentWriteRoles)]
    [HttpPost("orders/{orderId:int}/record")]
    public async Task<ActionResult<ApiResponse<PaymentResponse>>> RecordPayment(
        [FromRoute] int orderId,
        [FromBody] RecordPaymentRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new RecordPaymentCommand(
                orderId,
                request.Amount,
                request.PaymentMethodId,
                request.ReferenceNo,
                request.ChequeNo,
                request.ChequeBankName,
                request.ChequeDate),
            cancellationToken);

        return Ok(ApiResponse<PaymentResponse>.Ok(
            response,
            "Payment recorded successfully."));
    }

    [Authorize(Roles = PaymentWriteRoles)]
    [HttpPost("{paymentId:int}/mark-cheque-cleared")]
    public async Task<ActionResult<ApiResponse<PaymentResponse>>> MarkChequeCleared(
        [FromRoute] int paymentId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new MarkChequeClearedCommand(paymentId),
            cancellationToken);

        return Ok(ApiResponse<PaymentResponse>.Ok(
            response,
            "Cheque payment marked as cleared successfully."));
    }

    [Authorize(Roles = PaymentWriteRoles)]
    [HttpPost("{paymentId:int}/mark-cheque-rejected")]
    public async Task<ActionResult<ApiResponse<PaymentResponse>>> MarkChequeRejected(
        [FromRoute] int paymentId,
        [FromBody] MarkChequeRejectedRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new MarkChequeRejectedCommand(
                paymentId,
                request.RejectionReason),
            cancellationToken);

        return Ok(ApiResponse<PaymentResponse>.Ok(
            response,
            "Cheque payment marked as rejected successfully."));
    }
}