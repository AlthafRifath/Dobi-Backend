using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Services;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.ServicePrices.UpdateServicePriceStatus
{
    public sealed class UpdateServicePriceStatusCommandHandler
    : IRequestHandler<UpdateServicePriceStatusCommand, ServicePriceResponse>
    {
        private readonly IDobiDbContext _dbContext;

        public UpdateServicePriceStatusCommandHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ServicePriceResponse> Handle(
            UpdateServicePriceStatusCommand request,
            CancellationToken cancellationToken)
        {
            var servicePrice = await _dbContext.ServicePrices
                .Include(x => x.Service)
                .Include(x => x.ItemCategory)
                .Include(x => x.PricingType)
                .FirstOrDefaultAsync(x => x.Id == request.ServicePriceId, cancellationToken);

            if (servicePrice is null)
            {
                throw new NotFoundException("Service price", request.ServicePriceId);
            }

            servicePrice.IsActive = request.IsActive;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new ServicePriceResponse(
                servicePrice.Id,
                servicePrice.ServiceId,
                servicePrice.Service.ServiceName,
                servicePrice.ItemCategoryId,
                servicePrice.ItemCategory?.CategoryName,
                servicePrice.PricingTypeId,
                servicePrice.PricingType.PricingTypeName,
                servicePrice.BasePrice,
                servicePrice.ExpressAdditionalPrice,
                servicePrice.EffectiveFrom,
                servicePrice.EffectiveTo,
                servicePrice.IsActive);
        }
    }
}
