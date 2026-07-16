using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Notifications;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Notifications.GetNotificationsByOrder
{
    public sealed class GetNotificationsByOrderQueryHandler
    : IRequestHandler<GetNotificationsByOrderQuery, IReadOnlyCollection<NotificationResponse>>
    {
        private readonly IDobiDbContext _dbContext;

        public GetNotificationsByOrderQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyCollection<NotificationResponse>> Handle(
            GetNotificationsByOrderQuery request,
            CancellationToken cancellationToken)
        {
            var orderExists = await _dbContext.Orders
                .AnyAsync(x => x.Id == request.OrderId, cancellationToken);

            if (!orderExists)
            {
                throw new NotFoundException("Order", request.OrderId);
            }

            var notifications = await _dbContext.Notifications
                .AsNoTracking()
                .Include(x => x.Order)
                .Include(x => x.Customer)
                .Include(x => x.NotificationType)
                .Include(x => x.NotificationStatus)
                .Where(x => x.OrderId == request.OrderId)
                .OrderByDescending(x => x.CreatedAt)
                .ToArrayAsync(cancellationToken);

            return notifications
                .Select(NotificationResponseMapper.Map)
                .ToArray();
        }
    }
}
