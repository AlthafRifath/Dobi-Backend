using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.PlantProcessing.RecordQc
{
    public sealed class RecordQcCommandValidator : AbstractValidator<RecordQcCommand>
    {
        public RecordQcCommandValidator()
        {
            RuleFor(x => x.PlantProcessingId)
                .GreaterThan(0);

            RuleFor(x => x.OrderItemId)
                .GreaterThan(0)
                .When(x => x.OrderItemId.HasValue);

            RuleFor(x => x.QcStatusId)
                .GreaterThan(0);

            RuleFor(x => x.IssueDescription)
                .MaximumLength(500);

            RuleFor(x => x.ActionTaken)
                .MaximumLength(500);

            RuleFor(x => x.LabourChargeAmount)
                .GreaterThanOrEqualTo(0)
                .When(x => x.LabourChargeAmount.HasValue);
        }
    }
}
