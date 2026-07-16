using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Payments.MarkChequeRejected
{
    public sealed class MarkChequeRejectedCommandValidator : AbstractValidator<MarkChequeRejectedCommand>
    {
        public MarkChequeRejectedCommandValidator()
        {
            RuleFor(x => x.PaymentId)
                .GreaterThan(0);

            RuleFor(x => x.RejectionReason)
                .NotEmpty()
                .MaximumLength(500);
        }
    }
}
