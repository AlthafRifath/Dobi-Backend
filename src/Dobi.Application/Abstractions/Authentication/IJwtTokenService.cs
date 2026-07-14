using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Abstractions.Authentication
{
    public interface IJwtTokenService
    {
        AccessTokenResult GenerateAccessToken(
            int userId,
            string userName,
            string? email,
            IReadOnlyCollection<string> roles);

        string GenerateRefreshToken();
    }
}
