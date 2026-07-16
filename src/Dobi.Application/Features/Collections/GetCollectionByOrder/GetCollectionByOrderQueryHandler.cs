using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Collections;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Collections.GetCollectionByOrder
{
    public sealed class GetCollectionByOrderQueryHandler
    : IRequestHandler<GetCollectionByOrderQuery, OrderCollectionResponse>
    {
        private readonly IDobiDbContext _dbContext;

        public GetCollectionByOrderQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OrderCollectionResponse> Handle(
            GetCollectionByOrderQuery request,
            CancellationToken cancellationToken)
        {
            var order = await _dbContext.Orders
                .AsNoTracking()
                .Include(x => x.Customer)
                    .ThenInclude(x => x.CustomerType)
                .Include(x => x.Branch)
                .Include(x => x.CurrentStatus)
                .Include(x => x.PaymentStatus)
                .Include(x => x.Collection)
                    .ThenInclude(x => x!.CollectionMode)
                .FirstOrDefaultAsync(x => x.Id == request.OrderId, cancellationToken);

            if (order is null)
            {
                throw new NotFoundException("Order", request.OrderId);
            }

            if (order.Collection is null)
            {
                throw new NotFoundException("Order collection for order", request.OrderId);
            }

            return OrderCollectionResponseMapper.Map(order);
        }
    }
}
