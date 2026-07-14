using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Reports
{
    public sealed record DailyRevenueReportItemResponse(
        int OrderId,
        string OrderNo,
        string CustomerName,
        decimal TotalAmount,
        string PaymentStatus,
        DateTime OrderDate);
}
