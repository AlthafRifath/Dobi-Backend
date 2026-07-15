using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Common;
using Dobi.Contracts.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.ServicePrices.GetServicePrices
{
    public sealed class GetServicePricesQueryHandler
    : IRequestHandler<GetServicePricesQuery, PagedResponse<ServicePriceResponse>>
    {
        private readonly IDobiDbContext _dbContext;

        public GetServicePricesQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResponse<ServicePriceResponse>> Handle(
            GetServicePricesQuery request,
            CancellationToken cancellationToken)
        {
            var query = _dbContext.ServicePrices
                .AsNoTracking()
                .Include(x => x.Service)
                .Include(x => x.ItemCategory)
                .Include(x => x.PricingType)
                .AsQueryable();

            if (request.ServiceId.HasValue)
            {
                query = query.Where(x => x.ServiceId == request.ServiceId.Value);
            }

            if (request.ItemCategoryId.HasValue)
            {
                query = query.Where(x => x.ItemCategoryId == request.ItemCategoryId.Value);
            }

            if (request.PricingTypeId.HasValue)
            {
                query = query.Where(x => x.PricingTypeId == request.PricingTypeId.Value);
            }

            if (request.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == request.IsActive.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var prices = await query
                .OrderBy(x => x.Service.ServiceName)
                .ThenBy(x => x.ItemCategory != null ? x.ItemCategory.CategoryName : string.Empty)
                .ThenByDescending(x => x.EffectiveFrom)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
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
                .ToArrayAsync(cancellationToken);

            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            return new PagedResponse<ServicePriceResponse>(
                prices,
                totalCount,
                request.PageNumber,
                request.PageSize,
                totalPages,
                request.PageNumber > 1,
                request.PageNumber < totalPages);
        }
    }
}
