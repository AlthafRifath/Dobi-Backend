using Dobi.Domain.Branches;
using Dobi.Domain.Common;
using Dobi.Domain.Plants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.Transfers
{
    public class TransferBatch : AuditableEntity
    {
        public string TransferNo { get; set; } = string.Empty;

        public int TransferTypeId { get; set; }
        public TransferType TransferType { get; set; } = null!;

        public int? FromBranchId { get; set; }
        public Branch? FromBranch { get; set; }

        public int? FromPlantId { get; set; }
        public Plant? FromPlant { get; set; }

        public int? ToBranchId { get; set; }
        public Branch? ToBranch { get; set; }

        public int? ToPlantId { get; set; }
        public Plant? ToPlant { get; set; }

        public int DriverUserId { get; set; }

        public int TransferStatusId { get; set; }
        public TransferStatus TransferStatus { get; set; } = null!;

        public DateTime? SentAt { get; set; }
        public DateTime? ReceivedAt { get; set; }

        public string? Remarks { get; set; }

        public ICollection<TransferBatchItem> Items { get; set; } = new List<TransferBatchItem>();
        public ICollection<TransferAcknowledgement> Acknowledgements { get; set; } = new List<TransferAcknowledgement>();
    }
}
