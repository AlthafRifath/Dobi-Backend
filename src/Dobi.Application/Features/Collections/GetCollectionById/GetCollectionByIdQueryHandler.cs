using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Collections;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Collections.GetCollectionById
{
    public sealed class GetCollectionByIdQueryHandler
    : IRequestHandler<GetCollectionByIdQuery, OrderCollectionResponse>
    {
        private readonly IDobiDbContext _dbContext;

        public GetCollectionByIdQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OrderCollectionResponse> Handle(
            GetCollectionByIdQuery request,
            CancellationToken cancellationToken)
        {
            var collection = await _dbContext.OrderCollections
                .AsNoTracking()
                .Include(x => x.CollectionMode)
                .Include(x => x.Order)
                    .ThenInclude(x => x.Customer)
                        .ThenInclude(x => x.CustomerType)
                .Include(x => x.Order)
                    .ThenInclude(x => x.Branch)
                .Include(x => x.Order)
                    .ThenInclude(x => x.CurrentStatus)
                .Include(x => x.Order)
                    .ThenInclude(x => x.PaymentStatus)
                .FirstOrDefaultAsync(x => x.Id == request.OrderCollectionId, cancellationToken);

            if (collection is null)
            {
                throw new NotFoundException("Order collection", request.OrderCollectionId);
            }

            return OrderCollectionResponseMapper.Map(collection);
        }
    }
}
