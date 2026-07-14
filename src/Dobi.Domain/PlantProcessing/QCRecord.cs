using Dobi.Domain.Common;
using Dobi.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.PlantProcessing
{
    public class QCRecord : BaseEntity
    {
        public int PlantProcessingId { get; set; }
        public PlantProcessing PlantProcessing { get; set; } = null!;

        public int? OrderItemId { get; set; }
        public OrderItem? OrderItem { get; set; }

        public int QCStatusId { get; set; }
        public QCStatus QCStatus { get; set; } = null!;

        public string? IssueDescription { get; set; }
        public string? ActionTaken { get; set; }
        public decimal? LabourChargeAmount { get; set; }

        public int RecordedByUserId { get; set; }
        public DateTime RecordedAt { get; set; }
    }
}
