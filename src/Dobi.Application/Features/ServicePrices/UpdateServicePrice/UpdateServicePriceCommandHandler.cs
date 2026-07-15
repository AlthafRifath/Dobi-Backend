using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Services;
using Dobi.Domain.Services;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.ServicePrices.UpdateServicePrice
{
    public sealed class UpdateServicePriceCommandHandler
    : IRequestHandler<UpdateServicePriceCommand, ServicePriceResponse>
    {
        private readonly IDobiDbContext _dbContext;

        public UpdateServicePriceCommandHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ServicePriceResponse> Handle(
            UpdateServicePriceCommand request,
            CancellationToken cancellationToken)
        {
            var servicePrice = await _dbContext.ServicePrices
                .FirstOrDefaultAsync(x => x.Id == request.ServicePriceId, cancellationToken);

            if (servicePrice is null)
            {
                throw new NotFoundException("Service price", request.ServicePriceId);
            }

            var service = await _dbContext.Services
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.ServiceId, cancellationToken);

            if (service is null)
            {
                throw new NotFoundException("Service", request.ServiceId);
            }

            ItemCategory? itemCategory = null;

            if (request.ItemCategoryId.HasValue)
            {
                itemCategory = await _dbContext.ItemCategories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == request.ItemCategoryId.Value, cancellationToken);

                if (itemCategory is null)
                {
                    throw new NotFoundException("Item category", request.ItemCategoryId.Value);
                }
            }

            var pricingType = await _dbContext.PricingTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.PricingTypeId, cancellationToken);

            if (pricingType is null)
            {
                throw new NotFoundException("Pricing type", request.PricingTypeId);
            }

            var effectiveTo = request.EffectiveTo ?? DateOnly.MaxValue;

            var overlapExists = await _dbContext.ServicePrices.AnyAsync(
                x => x.Id != request.ServicePriceId &&
                     x.ServiceId == request.ServiceId &&
                     x.ItemCategoryId == request.ItemCategoryId &&
                     x.PricingTypeId == request.PricingTypeId &&
                     x.IsActive &&
                     x.EffectiveFrom <= effectiveTo &&
                     (x.EffectiveTo == null || x.EffectiveTo >= request.EffectiveFrom),
                cancellationToken);

            if (overlapExists)
            {
                throw new ConflictException("An active overlapping service price already exists for this service, item category, pricing type, and date range.");
            }

            servicePrice.ServiceId = request.ServiceId;
            servicePrice.ItemCategoryId = request.ItemCategoryId;
            servicePrice.PricingTypeId = request.PricingTypeId;
            servicePrice.BasePrice = request.BasePrice;
            servicePrice.ExpressAdditionalPrice = request.ExpressAdditionalPrice;
            servicePrice.EffectiveFrom = request.EffectiveFrom;
            servicePrice.EffectiveTo = request.EffectiveTo;
            servicePrice.IsActive = request.IsActive;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new ServicePriceResponse(
                servicePrice.Id,
                service.Id,
                service.ServiceName,
                itemCategory?.Id,
                itemCategory?.CategoryName,
                pricingType.Id,
                pricingType.PricingTypeName,
                servicePrice.BasePrice,
                servicePrice.ExpressAdditionalPrice,
                servicePrice.EffectiveFrom,
                servicePrice.EffectiveTo,
                servicePrice.IsActive);
        }
    }
}
