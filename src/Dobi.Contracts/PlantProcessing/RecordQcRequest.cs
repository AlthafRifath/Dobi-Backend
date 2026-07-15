namespace Dobi.Contracts.PlantProcessing;

public sealed record RecordQcRequest(
    int? OrderItemId,
    int QcStatusId,
    string? IssueDescription,
    string? ActionTaken,
    decimal? LabourChargeAmount);