using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.PlantProcessing.StartPlantProcessing
{
    public sealed class StartPlantProcessingCommandValidator : AbstractValidator<StartPlantProcessingCommand>
    {
        public StartPlantProcessingCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0);

            RuleFor(x => x.PlantId)
                .GreaterThan(0);

            RuleFor(x => x.PlantRemarks)
                .MaximumLength(500);
        }
    }
}
