using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Reports
{
    public sealed record UnpaidOrderReportResponse(
        int OrderId,
        string OrderNo,
        DateTime OrderDate,
        int CustomerId,
        string CustomerName,
        string CustomerMobileNo,
        int BranchId,
        string BranchName,
        decimal OrderTotalAmount,
        decimal PaidAmount,
        decimal PendingClearanceAmount,
        decimal OutstandingAmount,
        int PaymentStatusId,
        string PaymentStatusName,
        int CurrentOrderStatusId,
        string CurrentOrderStatusName);
}
