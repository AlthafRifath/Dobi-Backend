using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Reports
{
    public sealed record PlantWorkloadReportResponse(
        int PlantId,
        string PlantName,
        int TotalOrders,
        int ProcessingOrders,
        int QcPendingOrders,
        int QcFailedOrders,
        int PackedOrders,
        int ReadyForOutletReturnOrders);
}
