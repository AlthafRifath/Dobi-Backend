using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Abstractions.Authentication
{
    public sealed record IdentityUserSearchRequest(
        int PageNumber,
        int PageSize,
        string? SearchTerm,
        IReadOnlyCollection<string> RoleCodes,
        bool? IsActive,
        int? BranchId,
        int? PlantId);
}
