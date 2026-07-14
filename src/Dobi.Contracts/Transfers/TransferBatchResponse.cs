using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Transfers
{
    public sealed record TransferBatchResponse(
        int TransferBatchId,
        string TransferNo,
        int TransferTypeId,
        string TransferTypeName,
        int TransferStatusId,
        string TransferStatusName,
        int? FromBranchId,
        int? FromPlantId,
        int? ToBranchId,
        int? ToPlantId,
        int DriverUserId,
        DateTime? SentAt,
        DateTime? ReceivedAt,
        string? Remarks,
        IReadOnlyCollection<TransferBatchItemResponse> Items,
        IReadOnlyCollection<TransferAcknowledgementResponse> Acknowledgements);
}
