using Dobi.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.PlantProcessing
{
    public class ProcessingStage : BaseEntity
    {
        public string StageCode { get; set; } = string.Empty; // WASHING, DRYING, IRONING, QC, PACKING
        public string StageName { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<PlantProcessingStageUpdate> StageUpdates { get; set; } = new List<PlantProcessingStageUpdate>();
    }
}
