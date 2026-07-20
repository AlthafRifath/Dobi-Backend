using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Abstractions.Authentication
{
    public sealed record IdentityUserPlantAssignmentInfo(
        int PlantId,
        string PlantName);
}
