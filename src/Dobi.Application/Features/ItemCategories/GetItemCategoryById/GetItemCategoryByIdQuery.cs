using Dobi.Contracts.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.ItemCategories.GetItemCategoryById
{
    public sealed record GetItemCategoryByIdQuery(
        int ItemCategoryId) : IRequest<ItemCategoryResponse>;
}
