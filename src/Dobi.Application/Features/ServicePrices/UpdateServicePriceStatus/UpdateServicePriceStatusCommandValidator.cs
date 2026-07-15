using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.ServicePrices.UpdateServicePriceStatus
{
    public sealed class UpdateServicePriceStatusCommandValidator : AbstractValidator<UpdateServicePriceStatusCommand>
    {
        public UpdateServicePriceStatusCommandValidator()
        {
            RuleFor(x => x.ServicePriceId)
                .GreaterThan(0);
        }
    }
}
