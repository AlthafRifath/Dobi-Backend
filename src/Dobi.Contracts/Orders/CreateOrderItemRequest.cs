using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Orders
{
    public sealed record CreateOrderItemRequest(
        int ServiceId,
        int ItemCategoryId,
        int PricingTypeId,
        int? ServicePriceId,
        int Quantity,
        decimal? WeightKg,
        decimal UnitPrice,
        string? SpecialNotes,
        IReadOnlyCollection<string>? ManualTagNumbers);
}
