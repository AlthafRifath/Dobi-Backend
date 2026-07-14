using Dobi.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.PlantProcessing
{
    public class PlantProcessingStageUpdate : BaseEntity
    {
        public int PlantProcessingId { get; set; }
        public PlantProcessing PlantProcessing { get; set; } = null!;

        public int ProcessingStageId { get; set; }
        public ProcessingStage ProcessingStage { get; set; } = null!;

        public int ProcessingStageStatusId { get; set; }
        public ProcessingStageStatus ProcessingStageStatus { get; set; } = null!;

        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public int UpdatedByUserId { get; set; }
        public string? Remarks { get; set; }
    }
}
