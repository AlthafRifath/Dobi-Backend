using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Notifications;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Notifications.GetNotificationById
{
    public sealed class GetNotificationByIdQueryHandler
    : IRequestHandler<GetNotificationByIdQuery, NotificationResponse>
    {
        private readonly IDobiDbContext _dbContext;

        public GetNotificationByIdQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<NotificationResponse> Handle(
            GetNotificationByIdQuery request,
            CancellationToken cancellationToken)
        {
            var notification = await _dbContext.Notifications
                .AsNoTracking()
                .Include(x => x.Order)
                .Include(x => x.Customer)
                .Include(x => x.NotificationType)
                .Include(x => x.NotificationStatus)
                .FirstOrDefaultAsync(x => x.Id == request.NotificationId, cancellationToken);

            if (notification is null)
            {
                throw new NotFoundException("Notification", request.NotificationId);
            }

            return NotificationResponseMapper.Map(notification);
        }
    }
}
