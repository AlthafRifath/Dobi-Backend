using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Reports
{
    public sealed record PendingOrderReportResponse(
        int OrderId,
        string OrderNo,
        string CustomerName,
        string CustomerMobileNo,
        string CurrentStatus,
        DateTime OrderDate,
        DateOnly? ExpectedReturnDate);
}
