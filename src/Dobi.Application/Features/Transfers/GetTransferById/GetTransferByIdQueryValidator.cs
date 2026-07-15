using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Transfers.GetTransferById
{
    public sealed class GetTransferByIdQueryValidator : AbstractValidator<GetTransferByIdQuery>
    {
        public GetTransferByIdQueryValidator()
        {
            RuleFor(x => x.TransferBatchId)
                .GreaterThan(0);
        }
    }
}
