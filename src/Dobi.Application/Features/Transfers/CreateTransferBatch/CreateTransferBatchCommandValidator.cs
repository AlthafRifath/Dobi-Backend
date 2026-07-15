using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Transfers.CreateTransferBatch
{
    public sealed class CreateTransferBatchCommandValidator : AbstractValidator<CreateTransferBatchCommand>
    {
        public CreateTransferBatchCommandValidator()
        {
            RuleFor(x => x.TransferTypeId)
                .GreaterThan(0);

            RuleFor(x => x.DriverUserId)
                .GreaterThan(0);

            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage("At least one order is required for a transfer.");

            RuleFor(x => x.Remarks)
                .MaximumLength(500);

            RuleForEach(x => x.Items)
                .ChildRules(item =>
                {
                    item.RuleFor(x => x.OrderId)
                        .GreaterThan(0);

                    item.RuleFor(x => x.NoOfBags)
                        .GreaterThanOrEqualTo(0);

                    item.RuleFor(x => x.NoOfPieces)
                        .GreaterThanOrEqualTo(0);

                    item.RuleFor(x => x)
                        .Must(x => x.NoOfBags > 0 || x.NoOfPieces > 0)
                        .WithMessage("Either number of bags or number of pieces must be greater than zero.");

                    item.RuleFor(x => x.Remarks)
                        .MaximumLength(500);
                });
        }
    }
}
