namespace Dobi.Contracts.PlantProcessing;

public sealed record StartPlantProcessingRequest(
    int PlantId,
    string? PlantRemarks);