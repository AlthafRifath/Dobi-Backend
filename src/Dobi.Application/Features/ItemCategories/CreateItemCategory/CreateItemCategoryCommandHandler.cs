using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Services;
using Dobi.Domain.Services;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.ItemCategories.CreateItemCategory
{
    public sealed class CreateItemCategoryCommandHandler
    : IRequestHandler<CreateItemCategoryCommand, ItemCategoryResponse>
    {
        private readonly IDobiDbContext _dbContext;

        public CreateItemCategoryCommandHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ItemCategoryResponse> Handle(
            CreateItemCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var categoryName = request.CategoryName.Trim();

            var pricingType = await _dbContext.PricingTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.DefaultPricingTypeId, cancellationToken);

            if (pricingType is null)
            {
                throw new NotFoundException("Pricing type", request.DefaultPricingTypeId);
            }

            var duplicateExists = await _dbContext.ItemCategories.AnyAsync(
                x => x.CategoryName.ToLower() == categoryName.ToLower(),
                cancellationToken);

            if (duplicateExists)
            {
                throw new ConflictException("An item category with the same name already exists.");
            }

            var itemCategory = new ItemCategory
            {
                CategoryName = categoryName,
                DefaultPricingTypeId = request.DefaultPricingTypeId,
                IsSpecialItem = request.IsSpecialItem,
                IsActive = request.IsActive
            };

            _dbContext.ItemCategories.Add(itemCategory);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new ItemCategoryResponse(
                itemCategory.Id,
                itemCategory.CategoryName,
                itemCategory.DefaultPricingTypeId,
                pricingType.PricingTypeName,
                itemCategory.IsSpecialItem,
                itemCategory.IsActive);
        }
    }
}
