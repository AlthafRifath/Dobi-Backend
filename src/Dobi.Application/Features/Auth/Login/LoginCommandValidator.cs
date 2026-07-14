using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Auth.Login
{
    public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.UserNameOrEmail)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MaximumLength(200);
        }
    }
}
