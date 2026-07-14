using Dobi.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Transfers
{
    public class TransferStatus : BaseEntity
    {
        public string StatusCode { get; set; } = string.Empty; // CREATED, IN_TRANSIT, RECEIVED
        public string StatusName { get; set; } = string.Empty;

        public ICollection<TransferBatch> TransferBatches { get; set; } = new List<TransferBatch>();
    }
}
