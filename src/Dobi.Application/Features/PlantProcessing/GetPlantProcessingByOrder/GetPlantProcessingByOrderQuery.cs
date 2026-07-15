using Dobi.Contracts.PlantProcessing;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.PlantProcessing.GetPlantProcessingByOrder
{
    public sealed record GetPlantProcessingByOrderQuery(
        int OrderId) : IRequest<PlantProcessingResponse>;
}
