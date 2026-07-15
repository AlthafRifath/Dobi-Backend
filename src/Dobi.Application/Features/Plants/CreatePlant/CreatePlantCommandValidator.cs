using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Plants.CreatePlant
{
    public sealed class CreatePlantCommandValidator : AbstractValidator<CreatePlantCommand>
    {
        public CreatePlantCommandValidator()
        {
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
