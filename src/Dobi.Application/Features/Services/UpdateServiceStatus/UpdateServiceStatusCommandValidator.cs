using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Services.UpdateServiceStatus
{
    public sealed class UpdateServiceStatusCommandValidator : AbstractValidator<UpdateServiceStatusCommand>
    {
        public UpdateServiceStatusCommandValidator()
        {
            RuleFor(x => x.ServiceId)
                .GreaterThan(0);
        }
    }
}
