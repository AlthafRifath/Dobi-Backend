using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Refunds.MarkRefundPaid
{
    public sealed class MarkRefundPaidCommandValidator : AbstractValidator<MarkRefundPaidCommand>
    {
        public MarkRefundPaidCommandValidator()
        {
            RuleFor(x => x.RefundId)
                .GreaterThan(0);
        }
    }
}
