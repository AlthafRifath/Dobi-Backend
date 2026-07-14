using Dobi.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Domain.PlantProcessing
{
    public class ProcessingStageStatus : BaseEntity
    {
        public string StatusCode { get; set; } = string.Empty; // PENDING, IN_PROGRESS, DONE, FAILED, NOT_REQUIRED
        public string StatusName { get; set; } = string.Empty;

        public ICollection<PlantProcessingStageUpdate> StageUpdates { get; set; } = new List<PlantProcessingStageUpdate>();
    }
}
