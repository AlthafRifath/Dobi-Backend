using Dobi.Contracts.PlantProcessing;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.PlantProcessing.GetPlantProcessingById
{
    public sealed record GetPlantProcessingByIdQuery(
        int PlantProcessingId) : IRequest<PlantProcessingResponse>;
}
