using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.ServicePrices.GetServicePriceById
{
    public sealed class GetServicePriceByIdQueryValidator : AbstractValidator<GetServicePriceByIdQuery>
    {
        public GetServicePriceByIdQueryValidator()
        {
            RuleFor(x => x.ServicePriceId)
                .GreaterThan(0);
        }
    }
}
