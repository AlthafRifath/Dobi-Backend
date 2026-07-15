using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Branches.UpdateBranch
{
    public sealed class UpdateBranchCommandValidator : AbstractValidator<UpdateBranchCommand>
    {
        public UpdateBranchCommandValidator()
        {
            RuleFor(x => x.BranchId)
                .GreaterThan(0);

            RuleFor(x => x.BranchName)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Address)
                .MaximumLength(300);

            RuleFor(x => x.ContactNo)
                .MaximumLength(20);
        }
    }
}
