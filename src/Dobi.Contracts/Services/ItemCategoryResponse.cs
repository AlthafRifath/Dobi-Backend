using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Services
{
    public sealed record ItemCategoryResponse(
        int ItemCategoryId,
        string CategoryName,
        int DefaultPricingTypeId,
        string DefaultPricingTypeName,
        bool IsSpecialItem,
        bool IsActive);
}
