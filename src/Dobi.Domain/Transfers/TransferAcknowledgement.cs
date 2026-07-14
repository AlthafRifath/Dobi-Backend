using Dobi.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Transfers
{
    public class TransferAcknowledgement : BaseEntity
    {
        public int TransferBatchId { get; set; }
        public TransferBatch TransferBatch { get; set; } = null!;

        public int AcknowledgementTypeId { get; set; }
        public AcknowledgementType AcknowledgementType { get; set; } = null!;

        public int UserId { get; set; }

        public DateTime AcknowledgedAt { get; set; }
        public string? SignatureUrl { get; set; }
        public string? Remarks { get; set; }
    }
}
