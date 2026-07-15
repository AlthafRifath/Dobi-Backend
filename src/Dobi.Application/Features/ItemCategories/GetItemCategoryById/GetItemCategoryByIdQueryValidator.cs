using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.ItemCategories.GetItemCategoryById
{
    public sealed class GetItemCategoryByIdQueryValidator : AbstractValidator<GetItemCategoryByIdQuery>
    {
        public GetItemCategoryByIdQueryValidator()
        {
            RuleFor(x => x.ItemCategoryId)
                .GreaterThan(0);
        }
    }
}
