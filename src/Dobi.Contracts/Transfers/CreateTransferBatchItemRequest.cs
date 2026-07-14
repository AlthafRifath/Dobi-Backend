using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Transfers
{
    public sealed record CreateTransferBatchItemRequest(
        int OrderId,
        int NoOfBags,
        int NoOfPieces,
        string? Remarks);
}
