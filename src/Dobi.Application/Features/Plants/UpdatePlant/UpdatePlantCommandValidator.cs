using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Plants.UpdatePlant
{
    public sealed class UpdatePlantCommandValidator : AbstractValidator<UpdatePlantCommand>
    {
        public UpdatePlantCommandValidator()
        {
            RuleFor(x => x.PlantId)
                .GreaterThan(0);

            RuleFor(x => x.PlantName)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Address)
                .MaximumLength(300);

            RuleFor(x => x.OperatingHours)
                .MaximumLength(150);
        }
    }
}
