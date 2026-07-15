using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.ServicePrices.GetServicePrices
{
    public sealed class GetServicePricesQueryValidator : AbstractValidator<GetServicePricesQuery>
    {
        public GetServicePricesQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0);

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100);

            RuleFor(x => x.ServiceId)
                .GreaterThan(0)
                .When(x => x.ServiceId.HasValue);

            RuleFor(x => x.ItemCategoryId)
                .GreaterThan(0)
                .When(x => x.ItemCategoryId.HasValue);

            RuleFor(x => x.PricingTypeId)
                .GreaterThan(0)
                .When(x => x.PricingTypeId.HasValue);
        }
    }
}
