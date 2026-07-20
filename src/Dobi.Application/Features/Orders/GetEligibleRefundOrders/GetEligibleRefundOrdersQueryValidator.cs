using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Orders.GetEligibleRefundOrders
{
    public sealed class GetEligibleRefundOrdersQueryValidator : AbstractValidator<GetEligibleRefundOrdersQuery>
    {
        public GetEligibleRefundOrdersQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0);

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100);

            RuleFor(x => x.SearchTerm)
                .MaximumLength(150);

            RuleFor(x => x.CustomerId)
                .GreaterThan(0)
                .When(x => x.CustomerId.HasValue);

            RuleFor(x => x.BranchId)
                .GreaterThan(0)
                .When(x => x.BranchId.HasValue);

            RuleFor(x => x.CustomerTypeId)
                .GreaterThan(0)
                .When(x => x.CustomerTypeId.HasValue);
        }
    }
}
