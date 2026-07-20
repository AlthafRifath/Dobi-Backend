using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Abstractions.Authentication
{
    public sealed record IdentityUserRoleInfo(
        int Id,
        string Code,
        string Name);
}
