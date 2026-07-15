using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Common;
using Dobi.Contracts.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.ItemCategories.GetItemCategories
{
    public sealed class GetItemCategoriesQueryHandler
    : IRequestHandler<GetItemCategoriesQuery, PagedResponse<ItemCategoryResponse>>
    {
        private readonly IDobiDbContext _dbContext;

        public GetItemCategoriesQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResponse<ItemCategoryResponse>> Handle(
            GetItemCategoriesQuery request,
            CancellationToken cancellationToken)
        {
            var query = _dbContext.ItemCategories
                .AsNoTracking()
                .Include(x => x.DefaultPricingType)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.Trim().ToLower();

                query = query.Where(x =>
                    x.CategoryName.ToLower().Contains(searchTerm));
            }

            if (request.PricingTypeId.HasValue)
            {
                query = query.Where(x => x.DefaultPricingTypeId == request.PricingTypeId.Value);
            }

            if (request.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == request.IsActive.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var itemCategories = await query
                .OrderBy(x => x.CategoryName)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new ItemCategoryResponse(
                    x.Id,
                    x.CategoryName,
                    x.DefaultPricingTypeId,
                    x.DefaultPricingType.PricingTypeName,
                    x.IsSpecialItem,
                    x.IsActive))
                .ToArrayAsync(cancellationToken);

            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            return new PagedResponse<ItemCategoryResponse>(
                itemCategories,
                totalCount,
                request.PageNumber,
                request.PageSize,
                totalPages,
                request.PageNumber > 1,
                request.PageNumber < totalPages);
        }
    }
}
