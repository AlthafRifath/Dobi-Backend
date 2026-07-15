using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Transfers.GetTransfers
{
    public sealed class GetTransfersQueryValidator : AbstractValidator<GetTransfersQuery>
    {
        public GetTransfersQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0);

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100);

            RuleFor(x => x.SearchTerm)
                .MaximumLength(150);

            RuleFor(x => x.TransferTypeId)
                .GreaterThan(0)
                .When(x => x.TransferTypeId.HasValue);

            RuleFor(x => x.TransferStatusId)
                .GreaterThan(0)
                .When(x => x.TransferStatusId.HasValue);

            RuleFor(x => x.DriverUserId)
                .GreaterThan(0)
                .When(x => x.DriverUserId.HasValue);

            RuleFor(x => x.ToDate)
                .GreaterThanOrEqualTo(x => x.FromDate)
                .When(x => x.FromDate.HasValue && x.ToDate.HasValue);
        }
    }
}
