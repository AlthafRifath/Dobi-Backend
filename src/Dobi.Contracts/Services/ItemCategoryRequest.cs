using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Services
{
    public sealed record ItemCategoryRequest(
        string CategoryName,
        int DefaultPricingTypeId,
        bool IsSpecialItem,
        bool IsActive = true);
}
