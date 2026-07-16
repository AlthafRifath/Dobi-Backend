using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Refunds.RejectRefund
{
    public sealed class RejectRefundCommandValidator : AbstractValidator<RejectRefundCommand>
    {
        public RejectRefundCommandValidator()
        {
            RuleFor(x => x.RefundId)
                .GreaterThan(0);

            RuleFor(x => x.RejectionReason)
                .NotEmpty()
                .MaximumLength(500);
        }
    }
}
