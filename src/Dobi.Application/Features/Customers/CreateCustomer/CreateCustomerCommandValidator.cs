using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Customers.CreateCustomer
{
    public sealed class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.MobileNo)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.Address)
                .NotEmpty()
                .MaximumLength(300);

            RuleFor(x => x.Email)
                .MaximumLength(150)
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.CustomerTypeId)
                .GreaterThan(0);
        }
    }
}
