namespace Dobi.Contracts.PlantProcessing;

public sealed record PlantProcessingResponse(
    int PlantProcessingId,
    int OrderId,
    string OrderNo,
    int PlantId,
    string PlantName,
    DateTime? ReceivedAtPlant,
    DateOnly? ReadyDate,
    int? OverallQcStatusId,
    string? OverallQcStatusName,
    string? PlantRemarks,
    int CurrentOrderStatusId,
    string CurrentOrderStatusName,
    int? LatestProcessingStageId,
    string? LatestProcessingStageName,
    int? LatestProcessingStageStatusId,
    string? LatestProcessingStageStatusName,
    IReadOnlyCollection<PlantProcessingStageUpdateResponse> StageUpdates,
    IReadOnlyCollection<QcRecordResponse> QcRecords);