using Dobi.Contracts.Plants;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Plants.UpdatePlantStatus
{
    public sealed record UpdatePlantStatusCommand(
        int PlantId,
        bool IsActive) : IRequest<PlantResponse>;
}
