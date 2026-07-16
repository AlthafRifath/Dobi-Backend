using Dobi.Application.Features.Refunds.ApproveRefund;
using Dobi.Application.Features.Refunds.CreateRefund;
using Dobi.Application.Features.Refunds.GetRefundById;
using Dobi.Application.Features.Refunds.GetRefunds;
using Dobi.Application.Features.Refunds.MarkRefundPaid;
using Dobi.Application.Features.Refunds.RejectRefund;
using Dobi.Contracts.Common;
using Dobi.Contracts.Refunds;
using Dobi.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dobi.Api.Controllers;

[ApiController]
[Route("api/refunds")]
[Authorize(Roles = RefundReadRoles)]
public sealed class RefundsController : ControllerBase
{
    private const string RefundReadRoles =
        RoleCodes.Admin + "," +
        RoleCodes.OutletStaff + "," +
        RoleCodes.Manager + "," +
        RoleCodes.OperationsDirector;

    private const string RefundWriteRoles =
        RoleCodes.Admin + "," +
        RoleCodes.Manager;

    private readonly IMediator _mediator;

    public RefundsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<RefundResponse>>>> GetRefunds(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] int? orderId = null,
        [FromQuery] int? paymentId = null,
        [FromQuery] int? refundStatusId = null,
        [FromQuery] DateOnly? fromDate = null,
        [FromQuery] DateOnly? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(
            new GetRefundsQuery(
                pageNumber,
                pageSize,
                searchTerm,
                orderId,
                paymentId,
                refundStatusId,
                fromDate,
                toDate),
            cancellationToken);

        return Ok(ApiResponse<PagedResponse<RefundResponse>>.Ok(
            response,
            "Refunds loaded successfully."));
    }

    [HttpGet("{refundId:int}")]
    public async Task<ActionResult<ApiResponse<RefundResponse>>> GetRefundById(
        [FromRoute] int refundId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetRefundByIdQuery(refundId),
            cancellationToken);

        return Ok(ApiResponse<RefundResponse>.Ok(
            response,
            "Refund loaded successfully."));
    }

    [Authorize(Roles = RefundWriteRoles)]
    [HttpPost("orders/{orderId:int}/create")]
    public async Task<ActionResult<ApiResponse<RefundResponse>>> CreateRefund(
        [FromRoute] int orderId,
        [FromBody] CreateRefundRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new CreateRefundCommand(
                orderId,
                request.PaymentId,
                request.Amount,
                request.Reason),
            cancellationToken);

        return Ok(ApiResponse<RefundResponse>.Ok(
            response,
            "Refund request created successfully."));
    }

    [Authorize(Roles = RefundWriteRoles)]
    [HttpPost("{refundId:int}/approve")]
    public async Task<ActionResult<ApiResponse<RefundResponse>>> ApproveRefund(
        [FromRoute] int refundId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new ApproveRefundCommand(refundId),
            cancellationToken);

        return Ok(ApiResponse<RefundResponse>.Ok(
            response,
            "Refund approved successfully."));
    }

    [Authorize(Roles = RefundWriteRoles)]
    [HttpPost("{refundId:int}/mark-paid")]
    public async Task<ActionResult<ApiResponse<RefundResponse>>> MarkRefundPaid(
        [FromRoute] int refundId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new MarkRefundPaidCommand(refundId),
            cancellationToken);

        return Ok(ApiResponse<RefundResponse>.Ok(
            response,
            "Refund marked as paid successfully."));
    }

    [Authorize(Roles = RefundWriteRoles)]
    [HttpPost("{refundId:int}/reject")]
    public async Task<ActionResult<ApiResponse<RefundResponse>>> RejectRefund(
        [FromRoute] int refundId,
        [FromBody] RejectRefundRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new RejectRefundCommand(
                refundId,
                request.RejectionReason),
            cancellationToken);

        return Ok(ApiResponse<RefundResponse>.Ok(
            response,
            "Refund rejected successfully."));
    }
}