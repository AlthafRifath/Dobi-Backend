using Dobi.Contracts.PlantProcessing;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.PlantProcessing.MarkReadyForOutletReturn
{
    public sealed record MarkReadyForOutletReturnCommand(
        int PlantProcessingId,
        DateOnly ReadyDate,
        string? PlantRemarks) : IRequest<PlantProcessingResponse>;
}
