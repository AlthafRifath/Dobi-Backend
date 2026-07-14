using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.PlantProcessing
{
    public sealed record UpdatePlantProcessingRequest(
        DateOnly? ReadyDate,
        int? OverallQCStatusId,
        string? PlantRemarks);
}
