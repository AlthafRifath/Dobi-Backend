using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.ItemCategories.UpdateItemCategory
{
    public sealed class UpdateItemCategoryCommandValidator : AbstractValidator<UpdateItemCategoryCommand>
    {
        public UpdateItemCategoryCommandValidator()
        {
            RuleFor(x => x.ItemCategoryId)
                .GreaterThan(0);

            RuleFor(x => x.CategoryName)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.DefaultPricingTypeId)
                .GreaterThan(0);
        }
    }
}
