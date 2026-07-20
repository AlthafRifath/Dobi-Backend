using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.PlantProcessing
{
    public sealed record PlantProcessingOrderItemResponse(
        int OrderItemId,

        int ServiceId,
        string? ServiceName,

        int ItemCategoryId,
        string? ItemCategoryName,

        int PricingTypeId,
        string? PricingTypeCode,
        string? PricingTypeName,

        int Quantity,
        decimal? WeightKg,

        decimal UnitPrice,
        decimal LineAmount,
        decimal TotalPrice,

        string? SpecialNotes,
        IReadOnlyCollection<string> Tags);
}
