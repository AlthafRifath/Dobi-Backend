using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Refunds.ApproveRefund
{
    public sealed class ApproveRefundCommandValidator : AbstractValidator<ApproveRefundCommand>
    {
        public ApproveRefundCommandValidator()
        {
            RuleFor(x => x.RefundId)
                .GreaterThan(0);
        }
    }
}
