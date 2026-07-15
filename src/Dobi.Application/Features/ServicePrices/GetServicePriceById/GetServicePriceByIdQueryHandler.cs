using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Services;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.ServicePrices.GetServicePriceById
{
    public sealed class GetServicePriceByIdQueryHandler
    : IRequestHandler<GetServicePriceByIdQuery, ServicePriceResponse>
    {
        private readonly IDobiDbContext _dbContext;

        public GetServicePriceByIdQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ServicePriceResponse> Handle(
            GetServicePriceByIdQuery request,
            CancellationToken cancellationToken)
        {
            var price = await _dbContext.ServicePrices
                .AsNoTracking()
                .Include(x => x.Service)
                .Include(x => x.ItemCategory)
                .Include(x => x.PricingType)
                .Where(x => x.Id == request.ServicePriceId)
                .Select(x => new ServicePriceResponse(
                    x.Id,
                    x.ServiceId,
                    x.Service.ServiceName,
                    x.ItemCategoryId,
                    x.ItemCategory != null ? x.ItemCategory.CategoryName : null,
                    x.PricingTypeId,
                    x.PricingType.PricingTypeName,
                    x.BasePrice,
                    x.ExpressAdditionalPrice,
                    x.EffectiveFrom,
                    x.EffectiveTo,
                    x.IsActive))
                .FirstOrDefaultAsync(cancellationToken);

            if (price is null)
            {
                throw new NotFoundException("Service price", request.ServicePriceId);
            }

            return price;
        }
    }
}
