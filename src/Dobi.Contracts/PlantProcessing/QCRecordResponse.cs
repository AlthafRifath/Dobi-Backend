using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.PlantProcessing
{
    public sealed record QCRecordResponse(
        int QCRecordId,
        int? OrderItemId,
        int QCStatusId,
        string QCStatusName,
        string? IssueDescription,
        string? ActionTaken,
        decimal? LabourChargeAmount,
        int RecordedByUserId,
        DateTime RecordedAt);
}
