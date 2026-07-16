using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Reports
{
    public sealed record ReadyForCollectionReportResponse(
        int OrderId,
        string OrderNo,
        DateTime OrderDate,
        int CustomerId,
        string CustomerName,
        string CustomerMobileNo,
        string CustomerTypeCode,
        string CustomerTypeName,
        int BranchId,
        string BranchName,
        decimal TotalAmount,
        int PaymentStatusId,
        string PaymentStatusName);
}
