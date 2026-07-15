using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Services.UpdateService
{
    public sealed class UpdateServiceCommandValidator : AbstractValidator<UpdateServiceCommand>
    {
        public UpdateServiceCommandValidator()
        {
            RuleFor(x => x.ServiceId)
                .GreaterThan(0);

            RuleFor(x => x.ServiceCode)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.ServiceName)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Description)
                .MaximumLength(300);
        }
    }
}
