using Dobi.Contracts.Reports;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Reports.GetDailyRevenueReport
{
    public sealed record GetDailyRevenueReportQuery(
        DateOnly FromDate,
        DateOnly ToDate) : IRequest<IReadOnlyCollection<DailyRevenueReportResponse>>;
}
