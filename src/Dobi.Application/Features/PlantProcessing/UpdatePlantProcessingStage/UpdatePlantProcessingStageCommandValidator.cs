using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.PlantProcessing.UpdatePlantProcessingStage
{
    public sealed class UpdatePlantProcessingStageCommandValidator
    : AbstractValidator<UpdatePlantProcessingStageCommand>
    {
        public UpdatePlantProcessingStageCommandValidator()
        {
            RuleFor(x => x.PlantProcessingId)
                .GreaterThan(0);

            RuleFor(x => x.ProcessingStageId)
                .GreaterThan(0);

            RuleFor(x => x.ProcessingStageStatusId)
                .GreaterThan(0);

            RuleFor(x => x.Remarks)
                .MaximumLength(500);
        }
    }
}
