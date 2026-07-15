using Dobi.Contracts.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.ItemCategories.UpdateItemCategoryStatus
{
    public sealed record UpdateItemCategoryStatusCommand(
        int ItemCategoryId,
        bool IsActive) : IRequest<ItemCategoryResponse>;
}
