using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Services
{
    public sealed record ServicePriceRequest(
        int ServiceId,
        int? ItemCategoryId,
        int PricingTypeId,
        decimal BasePrice,
        decimal? ExpressAdditionalPrice,
        DateOnly EffectiveFrom,
        DateOnly? EffectiveTo,
        bool IsActive = true);
}
