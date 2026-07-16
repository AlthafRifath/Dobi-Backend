using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Refunds.GetRefunds
{
    public sealed class GetRefundsQueryValidator : AbstractValidator<GetRefundsQuery>
    {
        public GetRefundsQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0);

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100);

            RuleFor(x => x.SearchTerm)
                .MaximumLength(150);

            RuleFor(x => x.OrderId)
                .GreaterThan(0)
                .When(x => x.OrderId.HasValue);

            RuleFor(x => x.PaymentId)
                .GreaterThan(0)
                .When(x => x.PaymentId.HasValue);

            RuleFor(x => x.RefundStatusId)
                .GreaterThan(0)
                .When(x => x.RefundStatusId.HasValue);

            RuleFor(x => x.ToDate)
                .GreaterThanOrEqualTo(x => x.FromDate)
                .When(x => x.FromDate.HasValue && x.ToDate.HasValue);
        }
    }
}
