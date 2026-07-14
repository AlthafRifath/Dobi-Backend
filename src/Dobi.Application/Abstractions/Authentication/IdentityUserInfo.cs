using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Abstractions.Authentication
{
    public sealed record IdentityUserInfo(
        int UserId,
        string FullName,
        string UserName,
        string? Email,
        string? PhoneNumber,
        bool IsActive,
        int? DefaultBranchId,
        int? DefaultPlantId);
}
