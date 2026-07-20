using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Abstractions.Authentication
{
    public sealed record IdentityRoleInfo(
        int Id,
        string Code,
        string Name,
        string? Description,
        bool IsActive);
}
