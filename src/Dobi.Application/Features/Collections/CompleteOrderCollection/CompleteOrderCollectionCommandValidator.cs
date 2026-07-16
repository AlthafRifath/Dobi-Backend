using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Collections.CompleteOrderCollection
{
    public sealed class CompleteOrderCollectionCommandValidator
    : AbstractValidator<CompleteOrderCollectionCommand>
    {
        public CompleteOrderCollectionCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0);

            RuleFor(x => x.CollectorName)
                .MaximumLength(150);

            RuleFor(x => x.CollectorMobileNo)
                .MaximumLength(20);

            RuleFor(x => x.ReceiptImageUrl)
                .MaximumLength(500);

            RuleFor(x => x.CustomerSignatureUrl)
                .MaximumLength(500);

            RuleFor(x => x.Remarks)
                .MaximumLength(500);
        }
    }
}
