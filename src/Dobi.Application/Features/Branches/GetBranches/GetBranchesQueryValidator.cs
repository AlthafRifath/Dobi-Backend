using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Branches.GetBranches
{
    public sealed class GetBranchesQueryValidator : AbstractValidator<GetBranchesQuery>
    {
        public GetBranchesQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0);

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100);

            RuleFor(x => x.SearchTerm)
                .MaximumLength(150);
        }
    }
}
