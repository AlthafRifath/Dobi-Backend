using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.ItemCategories.CreateItemCategory
{
    public sealed class CreateItemCategoryCommandValidator : AbstractValidator<CreateItemCategoryCommand>
    {
        public CreateItemCategoryCommandValidator()
        {
            RuleFor(x => x.CategoryName)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.DefaultPricingTypeId)
                .GreaterThan(0);
        }
    }
}
