using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.PlantProcessing
{
    public sealed record PlantProcessingResponse(
        int PlantProcessingId,
        int OrderId,
        string OrderNo,
        int PlantId,
        string PlantName,
        DateTime? ReceivedAtPlant,
        DateOnly? ReadyDate,
        int? OverallQCStatusId,
        string? OverallQCStatusName,
        string? PlantRemarks,
        IReadOnlyCollection<ProcessingStageUpdateResponse> StageUpdates,
        IReadOnlyCollection<QCRecordResponse> QCRecords);
}
