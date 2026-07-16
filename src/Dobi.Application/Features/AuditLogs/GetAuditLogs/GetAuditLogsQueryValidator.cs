using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.AuditLogs.GetAuditLogs
{
    public sealed class GetAuditLogsQueryValidator : AbstractValidator<GetAuditLogsQuery>
    {
        public GetAuditLogsQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0);

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100);

            RuleFor(x => x.SearchTerm)
                .MaximumLength(150);

            RuleFor(x => x.EntityName)
                .MaximumLength(100);

            RuleFor(x => x.Action)
                .MaximumLength(50);

            RuleFor(x => x.EntityId)
                .GreaterThan(0)
                .When(x => x.EntityId.HasValue);

            RuleFor(x => x.PerformedByUserId)
                .GreaterThan(0)
                .When(x => x.PerformedByUserId.HasValue);

            RuleFor(x => x.ToDate)
                .GreaterThanOrEqualTo(x => x.FromDate)
                .When(x => x.FromDate.HasValue && x.ToDate.HasValue);
        }
    }
}
