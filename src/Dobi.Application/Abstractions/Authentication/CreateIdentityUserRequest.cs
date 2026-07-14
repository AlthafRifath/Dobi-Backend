using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Abstractions.Authentication
{
    public sealed record CreateIdentityUserRequest(
        string FullName,
        string UserName,
        string? Email,
        string? PhoneNumber,
        string Password,
        IReadOnlyCollection<string> Roles,
        int? DefaultBranchId,
        int? DefaultPlantId);
}
