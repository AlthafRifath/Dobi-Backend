using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Customers.UpdateCustomer
{
    public sealed class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0);

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
        }
    }
}
