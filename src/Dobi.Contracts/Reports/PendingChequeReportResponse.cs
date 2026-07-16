using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Reports
{
    public sealed record PendingChequeReportResponse(
        int PaymentId,
        int OrderId,
        string OrderNo,
        int CustomerId,
        string CustomerName,
        string CustomerMobileNo,
        decimal Amount,
        string? ChequeNo,
        string? ChequeBankName,
        DateOnly? ChequeDate,
        DateTime CreatedAt,
        string PaymentStatusName);
}
