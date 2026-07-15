using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Branches.CreateBranch
{
    public sealed class CreateBranchCommandValidator : AbstractValidator<CreateBranchCommand>
    {
        public CreateBranchCommandValidator()
        {
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
