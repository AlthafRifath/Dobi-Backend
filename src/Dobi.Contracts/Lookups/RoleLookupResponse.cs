using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Lookups
{
    public sealed record RoleLookupResponse(
        int Id,
        string Code,
        string Name,
        string? Description,
        bool IsActive);
}
