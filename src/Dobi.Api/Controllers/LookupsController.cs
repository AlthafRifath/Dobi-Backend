using Dobi.Application.Features.Lookups.GetLookupValues;
using Dobi.Contracts.Common;
using Dobi.Contracts.Lookups;
using Dobi.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dobi.Api.Controllers;

[ApiController]
[Route("api/lookups")]
[Authorize]
public sealed class LookupsController : ControllerBase
{
    private readonly IMediator _mediator;

    public LookupsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("customer-types")]
    public Task<ActionResult<ApiResponse<IReadOnlyCollection<LookupResponse>>>> GetCustomerTypes(
        CancellationToken cancellationToken)
    {
        return GetLookupAsync(
            LookupTypes.CustomerTypes,
            "Customer types loaded successfully.",
            cancellationToken);
    }

    [HttpGet("pricing-types")]
    public Task<ActionResult<ApiResponse<IReadOnlyCollection<LookupResponse>>>> GetPricingTypes(
        CancellationToken cancellationToken)
    {
        return GetLookupAsync(
            LookupTypes.PricingTypes,
            "Pricing types loaded successfully.",
            cancellationToken);
    }

    [HttpGet("services")]
    public Task<ActionResult<ApiResponse<IReadOnlyCollection<LookupResponse>>>> GetServices(
        CancellationToken cancellationToken)
    {
        return GetLookupAsync(
            LookupTypes.Services,
            "Services loaded successfully.",
            cancellationToken);
    }

    [HttpGet("item-categories")]
    public Task<ActionResult<ApiResponse<IReadOnlyCollection<LookupResponse>>>> GetItemCategories(
        CancellationToken cancellationToken)
    {
        return GetLookupAsync(
            LookupTypes.ItemCategories,
            "Item categories loaded successfully.",
            cancellationToken);
    }

    [HttpGet("order-statuses")]
    public Task<ActionResult<ApiResponse<IReadOnlyCollection<LookupResponse>>>> GetOrderStatuses(
        CancellationToken cancellationToken)
    {
        return GetLookupAsync(
            LookupTypes.OrderStatuses,
            "Order statuses loaded successfully.",
            cancellationToken);
    }

    [HttpGet("payment-statuses")]
    public Task<ActionResult<ApiResponse<IReadOnlyCollection<LookupResponse>>>> GetPaymentStatuses(
        CancellationToken cancellationToken)
    {
        return GetLookupAsync(
            LookupTypes.PaymentStatuses,
            "Payment statuses loaded successfully.",
            cancellationToken);
    }

    [HttpGet("inspection-issue-types")]
    public Task<ActionResult<ApiResponse<IReadOnlyCollection<LookupResponse>>>> GetInspectionIssueTypes(
        CancellationToken cancellationToken)
    {
        return GetLookupAsync(
            LookupTypes.InspectionIssueTypes,
            "Inspection issue types loaded successfully.",
            cancellationToken);
    }

    [HttpGet("transfer-types")]
    public Task<ActionResult<ApiResponse<IReadOnlyCollection<LookupResponse>>>> GetTransferTypes(
        CancellationToken cancellationToken)
    {
        return GetLookupAsync(
            LookupTypes.TransferTypes,
            "Transfer types loaded successfully.",
            cancellationToken);
    }

    [HttpGet("transfer-statuses")]
    public Task<ActionResult<ApiResponse<IReadOnlyCollection<LookupResponse>>>> GetTransferStatuses(
        CancellationToken cancellationToken)
    {
        return GetLookupAsync(
            LookupTypes.TransferStatuses,
            "Transfer statuses loaded successfully.",
            cancellationToken);
    }

    [HttpGet("acknowledgement-types")]
    public Task<ActionResult<ApiResponse<IReadOnlyCollection<LookupResponse>>>> GetAcknowledgementTypes(
        CancellationToken cancellationToken)
    {
        return GetLookupAsync(
            LookupTypes.AcknowledgementTypes,
            "Acknowledgement types loaded successfully.",
            cancellationToken);
    }

    [HttpGet("processing-stages")]
    public Task<ActionResult<ApiResponse<IReadOnlyCollection<LookupResponse>>>> GetProcessingStages(
        CancellationToken cancellationToken)
    {
        return GetLookupAsync(
            LookupTypes.ProcessingStages,
            "Processing stages loaded successfully.",
            cancellationToken);
    }

    [HttpGet("processing-stage-statuses")]
    public Task<ActionResult<ApiResponse<IReadOnlyCollection<LookupResponse>>>> GetProcessingStageStatuses(
        CancellationToken cancellationToken)
    {
        return GetLookupAsync(
            LookupTypes.ProcessingStageStatuses,
            "Processing stage statuses loaded successfully.",
            cancellationToken);
    }

    [HttpGet("qc-statuses")]
    public Task<ActionResult<ApiResponse<IReadOnlyCollection<LookupResponse>>>> GetQcStatuses(
        CancellationToken cancellationToken)
    {
        return GetLookupAsync(
            LookupTypes.QcStatuses,
            "QC statuses loaded successfully.",
            cancellationToken);
    }

    [HttpGet("collection-modes")]
    public Task<ActionResult<ApiResponse<IReadOnlyCollection<LookupResponse>>>> GetCollectionModes(
        CancellationToken cancellationToken)
    {
        return GetLookupAsync(
            LookupTypes.CollectionModes,
            "Collection modes loaded successfully.",
            cancellationToken);
    }

    [HttpGet("payment-methods")]
    public Task<ActionResult<ApiResponse<IReadOnlyCollection<LookupResponse>>>> GetPaymentMethods(
        CancellationToken cancellationToken)
    {
        return GetLookupAsync(
            LookupTypes.PaymentMethods,
            "Payment methods loaded successfully.",
            cancellationToken);
    }

    [HttpGet("refund-statuses")]
    public Task<ActionResult<ApiResponse<IReadOnlyCollection<LookupResponse>>>> GetRefundStatuses(
        CancellationToken cancellationToken)
    {
        return GetLookupAsync(
            LookupTypes.RefundStatuses,
            "Refund statuses loaded successfully.",
            cancellationToken);
    }

    [HttpGet("notification-types")]
    public Task<ActionResult<ApiResponse<IReadOnlyCollection<LookupResponse>>>> GetNotificationTypes(
        CancellationToken cancellationToken)
    {
        return GetLookupAsync(
            LookupTypes.NotificationTypes,
            "Notification types loaded successfully.",
            cancellationToken);
    }

    [HttpGet("notification-statuses")]
    public Task<ActionResult<ApiResponse<IReadOnlyCollection<LookupResponse>>>> GetNotificationStatuses(
        CancellationToken cancellationToken)
    {
        return GetLookupAsync(
            LookupTypes.NotificationStatuses,
            "Notification statuses loaded successfully.",
            cancellationToken);
    }

    private async Task<ActionResult<ApiResponse<IReadOnlyCollection<LookupResponse>>>> GetLookupAsync(
        string lookupType,
        string message,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetLookupValuesQuery(lookupType),
            cancellationToken);

        return Ok(ApiResponse<IReadOnlyCollection<LookupResponse>>.Ok(
            response,
            message));
    }
}