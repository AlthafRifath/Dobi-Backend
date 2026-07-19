using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Common;
using Dobi.Contracts.Notifications;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Dobi.Application.Features.Notifications.GetNotifications;

public sealed class GetNotificationsQueryHandler
    : IRequestHandler<GetNotificationsQuery, PagedResponse<NotificationResponse>>
{
    private readonly IDobiDbContext _dbContext;

    public GetNotificationsQueryHandler(IDobiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResponse<NotificationResponse>> Handle(
        GetNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.Notifications
            .AsNoTracking()
            .Include(x => x.Order)
            .Include(x => x.Customer)
            .Include(x => x.NotificationType)
            .Include(x => x.NotificationStatus)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm
                .Trim()
                .ToLower();

            query = query.Where(x =>
                x.Recipient.ToLower().Contains(searchTerm) ||
                x.Message.ToLower().Contains(searchTerm) ||
                x.Customer.FullName.ToLower().Contains(searchTerm) ||
                x.Customer.MobileNo.ToLower().Contains(searchTerm) ||
                (
                    x.Order != null &&
                    x.Order.OrderNo.ToLower().Contains(searchTerm)
                ));
        }

        if (request.CustomerId.HasValue)
        {
            query = query.Where(x =>
                x.CustomerId == request.CustomerId.Value);
        }

        if (request.OrderId.HasValue)
        {
            query = query.Where(x =>
                x.OrderId == request.OrderId.Value);
        }

        if (request.NotificationStatusId.HasValue)
        {
            query = query.Where(x =>
                x.NotificationStatusId ==
                request.NotificationStatusId.Value);
        }

        if (request.FromDate.HasValue)
        {
            var fromDateUtc = request.FromDate.Value.ToDateTime(
                TimeOnly.MinValue,
                DateTimeKind.Utc);

            query = query.Where(x =>
                x.CreatedAt >= fromDateUtc);
        }

        if (request.ToDate.HasValue)
        {
            var toDateExclusiveUtc = request.ToDate.Value
                .AddDays(1)
                .ToDateTime(
                    TimeOnly.MinValue,
                    DateTimeKind.Utc);

            query = query.Where(x =>
                x.CreatedAt < toDateExclusiveUtc);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var notifications = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToArrayAsync(cancellationToken);

        var items = notifications
            .Select(NotificationResponseMapper.Map)
            .ToArray();

        var totalPages = (int)Math.Ceiling(
            totalCount / (double)request.PageSize);

        return new PagedResponse<NotificationResponse>(
            items,
            totalCount,
            request.PageNumber,
            request.PageSize,
            totalPages,
            request.PageNumber > 1,
            request.PageNumber < totalPages);
    }
}