using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Payments.GetPaymentById
{
    public sealed class GetPaymentByIdQueryValidator : AbstractValidator<GetPaymentByIdQuery>
    {
        public GetPaymentByIdQueryValidator()
        {
            RuleFor(x => x.PaymentId)
                .GreaterThan(0);
        }
    }
}
