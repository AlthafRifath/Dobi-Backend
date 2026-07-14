using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.PlantProcessing
{
    public sealed record PlantRequest(
        string PlantName,
        string? Address,
        string? OperatingHours,
        bool IsActive = true);
}
