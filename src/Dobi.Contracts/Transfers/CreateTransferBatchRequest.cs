using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Transfers
{
    public sealed record CreateTransferBatchRequest(
        int TransferTypeId,
        int? FromBranchId,
        int? FromPlantId,
        int? ToBranchId,
        int? ToPlantId,
        int DriverUserId,
        IReadOnlyCollection<CreateTransferBatchItemRequest> Items,
        string? Remarks);
}
