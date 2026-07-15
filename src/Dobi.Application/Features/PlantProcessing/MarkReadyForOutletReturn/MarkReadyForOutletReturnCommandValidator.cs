using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.PlantProcessing.MarkReadyForOutletReturn
{
    public sealed class MarkReadyForOutletReturnCommandValidator
    : AbstractValidator<MarkReadyForOutletReturnCommand>
    {
        public MarkReadyForOutletReturnCommandValidator()
        {
            RuleFor(x => x.PlantProcessingId)
                .GreaterThan(0);

            RuleFor(x => x.PlantRemarks)
                .MaximumLength(500);
        }
    }
}
