using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.ItemCategories.GetItemCategories
{
    public sealed class GetItemCategoriesQueryValidator : AbstractValidator<GetItemCategoriesQuery>
    {
        public GetItemCategoriesQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0);

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100);

            RuleFor(x => x.SearchTerm)
                .MaximumLength(150);

            RuleFor(x => x.PricingTypeId)
                .GreaterThan(0)
                .When(x => x.PricingTypeId.HasValue);
        }
    }
}
