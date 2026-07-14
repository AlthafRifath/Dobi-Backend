using Dobi.Shared.Constants;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Users.CreateUser
{
    public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private static readonly string[] AllowedRoles =
        {
        RoleCodes.Admin,
        RoleCodes.OutletStaff,
        RoleCodes.PlantSupervisor,
        RoleCodes.Driver,
        RoleCodes.Manager,
        RoleCodes.OperationsDirector
        };

        public CreateUserCommandValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.UserName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .MaximumLength(150)
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(20);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(200);

            RuleFor(x => x.Roles)
                .NotEmpty()
                .WithMessage("At least one role is required.");

            RuleForEach(x => x.Roles)
                .Must(role => AllowedRoles.Contains(role))
                .WithMessage("Invalid role code.");
        }
    }
}
