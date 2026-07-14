using Dobi.Domain.Common;
using Dobi.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Transfers
{
    public class TransferBatchItem : BaseEntity
    {
        public int TransferBatchId { get; set; }
        public TransferBatch TransferBatch { get; set; } = null!;

        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public int NoOfBags { get; set; }
        public int NoOfPieces { get; set; }

        public string? Remarks { get; set; }
    }
}
