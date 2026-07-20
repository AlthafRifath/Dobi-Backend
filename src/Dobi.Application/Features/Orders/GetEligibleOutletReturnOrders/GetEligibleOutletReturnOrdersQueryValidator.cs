using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Orders.GetEligibleOutletReturnOrders
{
    public sealed class GetEligibleOutletReturnOrdersQueryValidator : AbstractValidator<GetEligibleOutletReturnOrdersQuery>
    {
        public GetEligibleOutletReturnOrdersQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0);

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100);

            RuleFor(x => x.SearchTerm)
                .MaximumLength(150);

            RuleFor(x => x.FromPlantId)
                .GreaterThan(0);

            RuleFor(x => x.ToBranchId)
                .GreaterThan(0);
        }
    }
}
