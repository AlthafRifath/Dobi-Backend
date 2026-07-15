using Dobi.Contracts.Common;
using Dobi.Contracts.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.ItemCategories.GetItemCategories
{
    public sealed record GetItemCategoriesQuery(
        int PageNumber,
        int PageSize,
        string? SearchTerm,
        int? PricingTypeId,
        bool? IsActive) : IRequest<PagedResponse<ItemCategoryResponse>>;
}
