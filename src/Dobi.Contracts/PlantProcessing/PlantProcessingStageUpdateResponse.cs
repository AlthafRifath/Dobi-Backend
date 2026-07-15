namespace Dobi.Contracts.PlantProcessing;

public sealed record PlantProcessingStageUpdateResponse(
    int PlantProcessingStageUpdateId,
    int ProcessingStageId,
    string ProcessingStageName,
    int ProcessingStageStatusId,
    string ProcessingStageStatusName,
    DateTime? StartedAt,
    DateTime? CompletedAt,
    int UpdatedByUserId,
    string? Remarks);