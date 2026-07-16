using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Reports.GetDailyRevenueReport
{
    public sealed class GetDailyRevenueReportQueryValidator : AbstractValidator<GetDailyRevenueReportQuery>
    {
        public GetDailyRevenueReportQueryValidator()
        {
            RuleFor(x => x.ToDate)
                .GreaterThanOrEqualTo(x => x.FromDate);
        }
    }
}
