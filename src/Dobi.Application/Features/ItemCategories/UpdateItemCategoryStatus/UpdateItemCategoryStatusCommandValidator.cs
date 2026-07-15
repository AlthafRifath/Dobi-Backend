using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.ItemCategories.UpdateItemCategoryStatus
{
    public sealed class UpdateItemCategoryStatusCommandValidator : AbstractValidator<UpdateItemCategoryStatusCommand>
    {
        public UpdateItemCategoryStatusCommandValidator()
        {
            RuleFor(x => x.ItemCategoryId)
                .GreaterThan(0);
        }
    }
}
