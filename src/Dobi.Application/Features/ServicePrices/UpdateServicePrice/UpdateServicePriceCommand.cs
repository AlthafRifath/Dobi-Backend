using Dobi.Contracts.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.ServicePrices.UpdateServicePrice
{
    public sealed record UpdateServicePriceCommand(
        int ServicePriceId,
        int ServiceId,
        int? ItemCategoryId,
        int PricingTypeId,
        decimal BasePrice,
        decimal? ExpressAdditionalPrice,
        DateOnly EffectiveFrom,
        DateOnly? EffectiveTo,
        bool IsActive) : IRequest<ServicePriceResponse>;
}
