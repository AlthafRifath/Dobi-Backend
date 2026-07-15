namespace Dobi.Contracts.PlantProcessing;

public sealed record MarkReadyForOutletReturnRequest(
    DateOnly ReadyDate,
    string? PlantRemarks);