using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Plants
{
    public sealed record PlantResponse(
        int PlantId,
        string PlantName,
        string? Address,
        string? OperatingHours,
        bool IsActive);
}
