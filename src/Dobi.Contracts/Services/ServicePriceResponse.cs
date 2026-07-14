using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Services
{
    public sealed record ServicePriceResponse(
        int ServicePriceId,
        int ServiceId,
        string ServiceName,
        int? ItemCategoryId,
        string? ItemCategoryName,
        int PricingTypeId,
        string PricingTypeName,
        decimal BasePrice,
        decimal? ExpressAdditionalPrice,
        DateOnly EffectiveFrom,
        DateOnly? EffectiveTo,
        bool IsActive);
}
