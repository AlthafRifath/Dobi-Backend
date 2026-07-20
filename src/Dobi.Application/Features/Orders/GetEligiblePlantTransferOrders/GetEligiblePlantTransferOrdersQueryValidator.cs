using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Orders.GetEligiblePlantTransferOrders
{
    public sealed class GetEligiblePlantTransferOrdersQueryValidator : AbstractValidator<GetEligiblePlantTransferOrdersQuery>
    {
        public GetEligiblePlantTransferOrdersQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0);

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100);

            RuleFor(x => x.SearchTerm)
                .MaximumLength(150);

            RuleFor(x => x.FromBranchId)
                .GreaterThan(0);

            RuleFor(x => x.ToPlantId)
                .GreaterThan(0);

            RuleFor(x => x.CustomerTypeId)
                .GreaterThan(0)
                .When(x => x.CustomerTypeId.HasValue);
        }
    }
}
