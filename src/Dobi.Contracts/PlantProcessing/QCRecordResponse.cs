namespace Dobi.Contracts.PlantProcessing;

public sealed record QcRecordResponse(
    int QcRecordId,
    int? OrderItemId,
    int QcStatusId,
    string QcStatusName,
    string? IssueDescription,
    string? ActionTaken,
    decimal? LabourChargeAmount,
    int RecordedByUserId,
    DateTime RecordedAt);