using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Orders
{
    public sealed record OrderItemResponse(
        int OrderItemId,
        int ServiceId,
        string ServiceName,
        int ItemCategoryId,
        string ItemCategoryName,
        int PricingTypeId,
        string PricingTypeName,
        int Quantity,
        decimal? WeightKg,
        decimal UnitPrice,
        decimal LineAmount,
        string? SpecialNotes,
        IReadOnlyCollection<string> ManualTagNumbers);
}
