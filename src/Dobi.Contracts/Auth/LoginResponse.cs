using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Auth
{
    public sealed record LoginResponse(
        int UserId,
        string FullName,
        string UserName,
        string? Email,
        IReadOnlyCollection<string> Roles,
        string AccessToken,
        string RefreshToken,
        DateTime AccessTokenExpiresAtUtc);
}
