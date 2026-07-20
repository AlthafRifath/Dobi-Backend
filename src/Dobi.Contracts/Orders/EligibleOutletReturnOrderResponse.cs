using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Orders
{
    public sealed class EligibleOutletReturnOrderResponse : EligibleOrderBaseResponse
    {
        public int SourcePlantId { get; init; }
        public string SourcePlantName { get; init; } = string.Empty;

        public int DestinationBranchId { get; init; }
        public string DestinationBranchName { get; init; } = string.Empty;

        public int PlantProcessingId { get; init; }
        public DateTime? ReadyForOutletReturnAt { get; init; }
    }
}
