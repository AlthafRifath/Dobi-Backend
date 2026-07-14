using Dobi.Domain.Common;
using Dobi.Domain.Orders;
using Dobi.Domain.Plants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.PlantProcessing
{
    public class PlantProcessing : AuditableEntity
    {
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public int PlantId { get; set; }
        public Plant Plant { get; set; } = null!;

        public DateTime? ReceivedAtPlant { get; set; }
        public DateOnly? ReadyDate { get; set; }

        public int? OverallQCStatusId { get; set; }
        public QCStatus? OverallQCStatus { get; set; }

        public string? PlantRemarks { get; set; }

        public ICollection<PlantProcessingStageUpdate> StageUpdates { get; set; } = new List<PlantProcessingStageUpdate>();
        public ICollection<QCRecord> QCRecords { get; set; } = new List<QCRecord>();
    }
}
