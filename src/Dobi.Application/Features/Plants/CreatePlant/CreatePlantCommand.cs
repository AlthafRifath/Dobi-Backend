using Dobi.Contracts.Plants;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Plants.CreatePlant
{
    public sealed record CreatePlantCommand(
        string PlantName,
        string? Address,
        string? OperatingHours,
        bool IsActive) : IRequest<PlantResponse>;
}
