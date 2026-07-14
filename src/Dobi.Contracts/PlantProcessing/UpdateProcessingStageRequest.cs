using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.PlantProcessing
{
    public sealed record UpdateProcessingStageRequest(
        int ProcessingStageId,
        int ProcessingStageStatusId,
        DateTime? StartedAt,
        DateTime? CompletedAt,
        string? Remarks);
}
