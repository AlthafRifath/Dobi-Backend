using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Customers.UpdateCustomerStatus
{
    public sealed class UpdateCustomerStatusCommandValidator : AbstractValidator<UpdateCustomerStatusCommand>
    {
        public UpdateCustomerStatusCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0);
        }
    }
}
