using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Services;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.ItemCategories.GetItemCategoryById
{
    public sealed class GetItemCategoryByIdQueryHandler
    : IRequestHandler<GetItemCategoryByIdQuery, ItemCategoryResponse>
    {
        private readonly IDobiDbContext _dbContext;

        public GetItemCategoryByIdQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ItemCategoryResponse> Handle(
            GetItemCategoryByIdQuery request,
            CancellationToken cancellationToken)
        {
            var itemCategory = await _dbContext.ItemCategories
                .AsNoTracking()
                .Include(x => x.DefaultPricingType)
                .Where(x => x.Id == request.ItemCategoryId)
                .Select(x => new ItemCategoryResponse(
                    x.Id,
                    x.CategoryName,
                    x.DefaultPricingTypeId,
                    x.DefaultPricingType.PricingTypeName,
                    x.IsSpecialItem,
                    x.IsActive))
                .FirstOrDefaultAsync(cancellationToken);

            if (itemCategory is null)
            {
                throw new NotFoundException("Item category", request.ItemCategoryId);
            }

            return itemCategory;
        }
    }
}
