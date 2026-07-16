using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Collections.MarkOrderReadyForCollection
{
    public sealed class MarkOrderReadyForCollectionCommandValidator
    : AbstractValidator<MarkOrderReadyForCollectionCommand>
    {
        public MarkOrderReadyForCollectionCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0);

            RuleFor(x => x.Remarks)
                .MaximumLength(500);
        }
    }
}
