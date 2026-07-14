using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Auth
{
    public sealed record CreateUserRequest(
        string FullName,
        string UserName,
        string? Email,
        string? PhoneNumber,
        string Password,
        IReadOnlyCollection<string> Roles,
        int? DefaultBranchId,
        int? DefaultPlantId);
}
