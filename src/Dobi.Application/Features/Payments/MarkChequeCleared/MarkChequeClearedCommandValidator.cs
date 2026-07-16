using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Payments.MarkChequeCleared
{
    public sealed class MarkChequeClearedCommandValidator : AbstractValidator<MarkChequeClearedCommand>
    {
        public MarkChequeClearedCommandValidator()
        {
            RuleFor(x => x.PaymentId)
                .GreaterThan(0);
        }
    }
}
