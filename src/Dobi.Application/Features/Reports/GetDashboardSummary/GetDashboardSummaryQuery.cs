using Dobi.Contracts.Reports;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Reports.GetDashboardSummary
{
    public sealed record GetDashboardSummaryQuery(
        DateOnly? FromDate,
        DateOnly? ToDate) : IRequest<DashboardSummaryResponse>;
}
