using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Plants.UpdatePlantStatus
{
    public sealed class UpdatePlantStatusCommandValidator : AbstractValidator<UpdatePlantStatusCommand>
    {
        public UpdatePlantStatusCommandValidator()
        {
            RuleFor(x => x.PlantId)
                .GreaterThan(0);
        }
    }
}
