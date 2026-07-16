using Dobi.Application.Features.Notifications.GetNotificationById;
using Dobi.Application.Features.Notifications.GetNotifications;
using Dobi.Application.Features.Notifications.GetNotificationsByOrder;
using Dobi.Application.Features.Notifications.RetryNotification;
using Dobi.Application.Features.Notifications.SendSmsNotification;
using Dobi.Contracts.Common;
using Dobi.Contracts.Notifications;
using Dobi.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dobi.Api.Controllers;

[ApiController]
[Route("api/notifications")]
[Authorize(Roles = NotificationReadRoles)]
public sealed class NotificationsController : ControllerBase
{
    private const string NotificationReadRoles =
        RoleCodes.Admin + "," +
        RoleCodes.OutletStaff + "," +
        RoleCodes.PlantSupervisor + "," +
        RoleCodes.Manager + "," +
        RoleCodes.OperationsDirector;

    private const string NotificationWriteRoles =
        RoleCodes.Admin + "," +
        RoleCodes.OutletStaff + "," +
        RoleCodes.Manager;

    private readonly IMediator _mediator;

    public NotificationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResponse<NotificationResponse>>>> GetNotifications(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? searchTerm = null,
        [FromQuery] int? customerId = null,
        [FromQuery] int? orderId = null,
        [FromQuery] int? notificationStatusId = null,
        [FromQuery] DateOnly? fromDate = null,
        [FromQuery] DateOnly? toDate = null,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(
            new GetNotificationsQuery(
                pageNumber,
                pageSize,
                searchTerm,
                customerId,
                orderId,
                notificationStatusId,
                fromDate,
                toDate),
            cancellationToken);

        return Ok(ApiResponse<PagedResponse<NotificationResponse>>.Ok(
            response,
            "Notifications loaded successfully."));
    }

    [HttpGet("{notificationId:int}")]
    public async Task<ActionResult<ApiResponse<NotificationResponse>>> GetNotificationById(
        [FromRoute] int notificationId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetNotificationByIdQuery(notificationId),
            cancellationToken);

        return Ok(ApiResponse<NotificationResponse>.Ok(
            response,
            "Notification loaded successfully."));
    }

    [HttpGet("by-order/{orderId:int}")]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<NotificationResponse>>>> GetNotificationsByOrder(
        [FromRoute] int orderId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new GetNotificationsByOrderQuery(orderId),
            cancellationToken);

        return Ok(ApiResponse<IReadOnlyCollection<NotificationResponse>>.Ok(
            response,
            "Order notifications loaded successfully."));
    }

    [Authorize(Roles = NotificationWriteRoles)]
    [HttpPost("sms/send")]
    public async Task<ActionResult<ApiResponse<NotificationResponse>>> SendSms(
        [FromBody] SendSmsNotificationRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new SendSmsNotificationCommand(
                request.OrderId,
                request.CustomerId,
                request.RecipientMobileNo,
                request.Message),
            cancellationToken);

        return Ok(ApiResponse<NotificationResponse>.Ok(
            response,
            "SMS notification processed successfully."));
    }

    [Authorize(Roles = NotificationWriteRoles)]
    [HttpPost("{notificationId:int}/retry")]
    public async Task<ActionResult<ApiResponse<NotificationResponse>>> RetryNotification(
        [FromRoute] int notificationId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new RetryNotificationCommand(notificationId),
            cancellationToken);

        return Ok(ApiResponse<NotificationResponse>.Ok(
            response,
            "Notification retry processed successfully."));
    }
}