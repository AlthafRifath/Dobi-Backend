using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Orders
{
    public sealed class EligiblePlantTransferOrderResponse : EligibleOrderBaseResponse
    {
        public int SourceBranchId { get; init; }
        public string SourceBranchName { get; init; } = string.Empty;

        public int DestinationPlantId { get; init; }
        public string DestinationPlantName { get; init; } = string.Empty;
    }
}
