using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Services;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.ItemCategories.UpdateItemCategoryStatus
{
    public sealed class UpdateItemCategoryStatusCommandHandler
    : IRequestHandler<UpdateItemCategoryStatusCommand, ItemCategoryResponse>
    {
        private readonly IDobiDbContext _dbContext;

        public UpdateItemCategoryStatusCommandHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ItemCategoryResponse> Handle(
            UpdateItemCategoryStatusCommand request,
            CancellationToken cancellationToken)
        {
            var itemCategory = await _dbContext.ItemCategories
                .Include(x => x.DefaultPricingType)
                .FirstOrDefaultAsync(x => x.Id == request.ItemCategoryId, cancellationToken);

            if (itemCategory is null)
            {
                throw new NotFoundException("Item category", request.ItemCategoryId);
            }

            itemCategory.IsActive = request.IsActive;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new ItemCategoryResponse(
                itemCategory.Id,
                itemCategory.CategoryName,
                itemCategory.DefaultPricingTypeId,
                itemCategory.DefaultPricingType.PricingTypeName,
                itemCategory.IsSpecialItem,
                itemCategory.IsActive);
        }
    }
}
