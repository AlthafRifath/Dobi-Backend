using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Orders;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Orders.GetOrderStatusHistory
{
    public sealed class GetOrderStatusHistoryQueryHandler
    : IRequestHandler<GetOrderStatusHistoryQuery, IReadOnlyCollection<OrderStatusHistoryResponse>>
    {
        private readonly IDobiDbContext _dbContext;

        public GetOrderStatusHistoryQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyCollection<OrderStatusHistoryResponse>> Handle(
            GetOrderStatusHistoryQuery request,
            CancellationToken cancellationToken)
        {
            var orderExists = await _dbContext.Orders.AnyAsync(
                x => x.Id == request.OrderId,
                cancellationToken);

            if (!orderExists)
            {
                throw new NotFoundException("Order", request.OrderId);
            }

            return await _dbContext.OrderStatusHistories
                .AsNoTracking()
                .Include(x => x.OrderStatus)
                .Where(x => x.OrderId == request.OrderId)
                .OrderBy(x => x.ChangedAt)
                .Select(x => new OrderStatusHistoryResponse(
                    x.Id,
                    x.OrderId,
                    x.OrderStatusId,
                    x.OrderStatus.StatusName,
                    x.Remarks,
                    x.ChangedByUserId,
                    x.ChangedAt))
                .ToArrayAsync(cancellationToken);
        }
    }
}
