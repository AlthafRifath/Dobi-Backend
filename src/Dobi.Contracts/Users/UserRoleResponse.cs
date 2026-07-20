using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Users
{
    public sealed record UserRoleResponse(
        int Id,
        string Code,
        string Name);
}
