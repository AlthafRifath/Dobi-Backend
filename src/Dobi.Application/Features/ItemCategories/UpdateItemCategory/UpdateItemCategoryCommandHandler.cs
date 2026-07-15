using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Services;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.ItemCategories.UpdateItemCategory
{
    public sealed class UpdateItemCategoryCommandHandler
    : IRequestHandler<UpdateItemCategoryCommand, ItemCategoryResponse>
    {
        private readonly IDobiDbContext _dbContext;

        public UpdateItemCategoryCommandHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ItemCategoryResponse> Handle(
            UpdateItemCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var itemCategory = await _dbContext.ItemCategories
                .FirstOrDefaultAsync(x => x.Id == request.ItemCategoryId, cancellationToken);

            if (itemCategory is null)
            {
                throw new NotFoundException("Item category", request.ItemCategoryId);
            }

            var pricingType = await _dbContext.PricingTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.DefaultPricingTypeId, cancellationToken);

            if (pricingType is null)
            {
                throw new NotFoundException("Pricing type", request.DefaultPricingTypeId);
            }

            var categoryName = request.CategoryName.Trim();

            var duplicateExists = await _dbContext.ItemCategories.AnyAsync(
                x => x.Id != request.ItemCategoryId &&
                     x.CategoryName.ToLower() == categoryName.ToLower(),
                cancellationToken);

            if (duplicateExists)
            {
                throw new ConflictException("An item category with the same name already exists.");
            }

            itemCategory.CategoryName = categoryName;
            itemCategory.DefaultPricingTypeId = request.DefaultPricingTypeId;
            itemCategory.IsSpecialItem = request.IsSpecialItem;
            itemCategory.IsActive = request.IsActive;

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
