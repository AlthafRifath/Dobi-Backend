using Dobi.Contracts.Common;
using Dobi.Contracts.Notifications;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Notifications.GetNotifications
{
    public sealed record GetNotificationsQuery(
        int PageNumber,
        int PageSize,
        string? SearchTerm,
        int? CustomerId,
        int? OrderId,
        int? NotificationStatusId,
        DateOnly? FromDate,
        DateOnly? ToDate) : IRequest<PagedResponse<NotificationResponse>>;
}
