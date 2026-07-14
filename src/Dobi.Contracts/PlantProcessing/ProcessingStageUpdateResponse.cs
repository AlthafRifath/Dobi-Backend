using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.PlantProcessing
{
    public sealed record ProcessingStageUpdateResponse(
        int PlantProcessingStageUpdateId,
        int ProcessingStageId,
        string ProcessingStageName,
        int ProcessingStageStatusId,
        string ProcessingStageStatusName,
        DateTime? StartedAt,
        DateTime? CompletedAt,
        int UpdatedByUserId,
        string? Remarks);
}
