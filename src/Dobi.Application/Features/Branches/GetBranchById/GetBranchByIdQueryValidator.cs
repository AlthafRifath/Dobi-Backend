using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Branches.GetBranchById
{
    public sealed class GetBranchByIdQueryValidator : AbstractValidator<GetBranchByIdQuery>
    {
        public GetBranchByIdQueryValidator()
        {
            RuleFor(x => x.BranchId)
                .GreaterThan(0);
        }
    }
}
