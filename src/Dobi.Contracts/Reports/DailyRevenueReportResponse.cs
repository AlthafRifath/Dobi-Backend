using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Reports
{
    public sealed record DailyRevenueReportResponse(
        DateOnly Date,
        decimal CashAmount,
        decimal CardAmount,
        decimal BankTransferAmount,
        decimal ChequeClearedAmount,
        decimal TotalPaidAmount,
        decimal RefundedAmount,
        decimal NetRevenue);
}
