using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Transfers
{
    public sealed record TransferBatchItemResponse(
        int TransferBatchItemId,
        int OrderId,
        string OrderNo,
        int NoOfBags,
        int NoOfPieces,
        string? Remarks);
}
