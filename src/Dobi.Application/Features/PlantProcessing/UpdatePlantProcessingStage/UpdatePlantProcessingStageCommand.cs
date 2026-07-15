using Dobi.Contracts.PlantProcessing;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.PlantProcessing.UpdatePlantProcessingStage
{
    public sealed record UpdatePlantProcessingStageCommand(
        int PlantProcessingId,
        int ProcessingStageId,
        int ProcessingStageStatusId,
        string? Remarks) : IRequest<PlantProcessingResponse>;
}
