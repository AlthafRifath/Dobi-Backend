using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Auth
{
    public sealed record UserPlantAssignmentRequest(
        int UserId,
        int PlantId,
        bool IsPrimary);
}
