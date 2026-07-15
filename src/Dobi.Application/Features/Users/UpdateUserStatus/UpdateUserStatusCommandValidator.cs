using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Users.UpdateUserStatus
{
    public sealed class UpdateUserStatusCommandValidator : AbstractValidator<UpdateUserStatusCommand>
    {
        public UpdateUserStatusCommandValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0);
        }
    }
}
