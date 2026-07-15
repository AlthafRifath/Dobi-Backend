using Dobi.Contracts.PlantProcessing;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.PlantProcessing.StartPlantProcessing
{
    public sealed record StartPlantProcessingCommand(
        int OrderId,
        int PlantId,
        string? PlantRemarks) : IRequest<PlantProcessingResponse>;
}
