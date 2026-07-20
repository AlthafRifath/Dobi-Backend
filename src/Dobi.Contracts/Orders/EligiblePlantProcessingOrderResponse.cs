using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Orders
{
    public sealed class EligiblePlantProcessingOrderResponse : EligibleOrderBaseResponse
    {
        public int BranchId { get; init; }
        public string BranchName { get; init; } = string.Empty;

        public int PlantId { get; init; }
        public string PlantName { get; init; } = string.Empty;

        public int ReceivedTransferBatchId { get; init; }
        public string ReceivedTransferNo { get; init; } = string.Empty;

        public DateTime? ReceivedAtPlantAt { get; init; }
    }
}
