using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.PlantProcessing
{
    public sealed record CreatePlantProcessingRequest(
        int OrderId,
        int PlantId,
        DateTime? ReceivedAtPlant,
        string? PlantRemarks);
}
