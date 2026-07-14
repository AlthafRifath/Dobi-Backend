using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.PlantProcessing
{
    public sealed record CreateQCRecordRequest(
        int? OrderItemId,
        int QCStatusId,
        string? IssueDescription,
        string? ActionTaken,
        decimal? LabourChargeAmount);
}
