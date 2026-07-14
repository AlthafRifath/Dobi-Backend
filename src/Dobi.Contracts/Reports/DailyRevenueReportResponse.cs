using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Reports
{
    public sealed record DailyRevenueReportResponse(
        DateOnly ReportDate,
        decimal TotalRevenue,
        int PaidOrderCount,
        int UnpaidOrderCount,
        IReadOnlyCollection<DailyRevenueReportItemResponse> Items);
}
