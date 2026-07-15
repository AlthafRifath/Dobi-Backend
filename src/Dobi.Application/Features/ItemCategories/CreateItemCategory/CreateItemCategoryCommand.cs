using Dobi.Contracts.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.ItemCategories.CreateItemCategory
{
    public sealed record CreateItemCategoryCommand(
        string CategoryName,
        int DefaultPricingTypeId,
        bool IsSpecialItem,
        bool IsActive) : IRequest<ItemCategoryResponse>;
}
