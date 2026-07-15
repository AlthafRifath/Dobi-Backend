using Dobi.Contracts.Plants;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Plants.UpdatePlant
{
    public sealed record UpdatePlantCommand(
        int PlantId,
        string PlantName,
        string? Address,
        string? OperatingHours,
        bool IsActive) : IRequest<PlantResponse>;
}
