using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Refunds.GetRefundById
{
    public sealed class GetRefundByIdQueryValidator : AbstractValidator<GetRefundByIdQuery>
    {
        public GetRefundByIdQueryValidator()
        {
            RuleFor(x => x.RefundId)
                .GreaterThan(0);
        }
    }
}
