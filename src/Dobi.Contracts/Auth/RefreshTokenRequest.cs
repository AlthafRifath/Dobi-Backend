using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Auth
{
    public sealed record RefreshTokenRequest(
        int UserId,
        string RefreshToken);
}
