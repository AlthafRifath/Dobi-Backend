using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Transfers.AcknowledgeTransfer
{
    public sealed class AcknowledgeTransferCommandValidator : AbstractValidator<AcknowledgeTransferCommand>
    {
        public AcknowledgeTransferCommandValidator()
        {
            RuleFor(x => x.TransferBatchId)
                .GreaterThan(0);

            RuleFor(x => x.AcknowledgementTypeId)
                .GreaterThan(0);

            RuleFor(x => x.SignatureUrl)
                .MaximumLength(500);

            RuleFor(x => x.Remarks)
                .MaximumLength(500);
        }
    }
}
