using Dobi.Contracts.Plants;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Plants.GetPlantById
{
    public sealed record GetPlantByIdQuery(
        int PlantId) : IRequest<PlantResponse>;
}
