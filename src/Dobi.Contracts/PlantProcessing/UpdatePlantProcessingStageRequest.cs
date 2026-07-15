namespace Dobi.Contracts.PlantProcessing;

public sealed record UpdatePlantProcessingStageRequest(
    int ProcessingStageId,
    int ProcessingStageStatusId,
    string? Remarks);