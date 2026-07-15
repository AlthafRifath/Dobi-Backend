using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Branches.UpdateBranchStatus
{
    public sealed class UpdateBranchStatusCommandValidator : AbstractValidator<UpdateBranchStatusCommand>
    {
        public UpdateBranchStatusCommandValidator()
        {
            RuleFor(x => x.BranchId)
                .GreaterThan(0);
        }
    }
}
