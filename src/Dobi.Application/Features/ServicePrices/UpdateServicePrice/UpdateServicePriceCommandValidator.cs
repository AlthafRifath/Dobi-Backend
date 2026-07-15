using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.ServicePrices.UpdateServicePrice
{
    public sealed class UpdateServicePriceCommandValidator : AbstractValidator<UpdateServicePriceCommand>
    {
        public UpdateServicePriceCommandValidator()
        {
            RuleFor(x => x.ServicePriceId)
                .GreaterThan(0);

            RuleFor(x => x.ServiceId)
                .GreaterThan(0);

            RuleFor(x => x.ItemCategoryId)
                .GreaterThan(0)
                .When(x => x.ItemCategoryId.HasValue);

            RuleFor(x => x.PricingTypeId)
                .GreaterThan(0);

            RuleFor(x => x.BasePrice)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.ExpressAdditionalPrice)
                .GreaterThanOrEqualTo(0)
                .When(x => x.ExpressAdditionalPrice.HasValue);

            RuleFor(x => x.EffectiveTo)
                .GreaterThanOrEqualTo(x => x.EffectiveFrom)
                .When(x => x.EffectiveTo.HasValue);
        }
    }
}
