using Dobi.Contracts.PlantProcessing;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.PlantProcessing.RecordQc
{
    public sealed record RecordQcCommand(
        int PlantProcessingId,
        int? OrderItemId,
        int QcStatusId,
        string? IssueDescription,
        string? ActionTaken,
        decimal? LabourChargeAmount) : IRequest<PlantProcessingResponse>;
}
