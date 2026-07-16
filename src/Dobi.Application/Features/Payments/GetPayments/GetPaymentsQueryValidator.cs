using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Payments.GetPayments
{
    public sealed class GetPaymentsQueryValidator : AbstractValidator<GetPaymentsQuery>
    {
        public GetPaymentsQueryValidator()
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

            RuleFor(x => x.PaymentMethodId)
                .GreaterThan(0)
                .When(x => x.PaymentMethodId.HasValue);

            RuleFor(x => x.PaymentStatusId)
                .GreaterThan(0)
                .When(x => x.PaymentStatusId.HasValue);

            RuleFor(x => x.ToDate)
                .GreaterThanOrEqualTo(x => x.FromDate)
                .When(x => x.FromDate.HasValue && x.ToDate.HasValue);
        }
    }
}
