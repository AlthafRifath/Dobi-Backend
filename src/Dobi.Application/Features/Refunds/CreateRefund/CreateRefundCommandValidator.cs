using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Refunds.CreateRefund
{
    public sealed class CreateRefundCommandValidator : AbstractValidator<CreateRefundCommand>
    {
        public CreateRefundCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0);

            RuleFor(x => x.PaymentId)
                .GreaterThan(0)
                .When(x => x.PaymentId.HasValue);

            RuleFor(x => x.Amount)
                .GreaterThan(0);

            RuleFor(x => x.Reason)
                .NotEmpty()
                .MaximumLength(500);
        }
    }
}
