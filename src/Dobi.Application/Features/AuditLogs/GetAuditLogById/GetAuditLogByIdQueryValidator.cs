using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.AuditLogs.GetAuditLogById
{
    public sealed class GetAuditLogByIdQueryValidator : AbstractValidator<GetAuditLogByIdQuery>
    {
        public GetAuditLogByIdQueryValidator()
        {
            RuleFor(x => x.AuditLogId)
                .GreaterThan(0);
        }
    }
}
