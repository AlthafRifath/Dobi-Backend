using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Reports
{
    public sealed record DashboardSummaryResponse(
        int TotalOrders,
        int ActiveOrders,
        int ReadyForCollectionOrders,
        int CollectedDeliveredOrders,
        int ClosedOrders,
        int PendingPlantOrders,

        decimal TotalOrderValue,
        decimal PaidAmount,
        decimal PendingClearanceAmount,
        decimal RefundedAmount,
        decimal NetRevenue,

        int PendingChequeCount,
        decimal PendingChequeAmount);
}
