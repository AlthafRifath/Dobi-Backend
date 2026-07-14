using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Auth
{
    public sealed record UserResponse(
        int UserId,
        string FullName,
        string UserName,
        string? Email,
        string? PhoneNumber,
        bool IsActive,
        IReadOnlyCollection<string> Roles,
        int? DefaultBranchId,
        int? DefaultPlantId);
}
