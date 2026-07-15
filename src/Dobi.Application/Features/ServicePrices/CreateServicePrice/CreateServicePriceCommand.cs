using Dobi.Contracts.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.ServicePrices.CreateServicePrice
{
    public sealed record CreateServicePriceCommand(
        int ServiceId,
        int? ItemCategoryId,
        int PricingTypeId,
        decimal BasePrice,
        decimal? ExpressAdditionalPrice,
        DateOnly EffectiveFrom,
        DateOnly? EffectiveTo,
        bool IsActive) : IRequest<ServicePriceResponse>;
}
