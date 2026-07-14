using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Abstractions.Authentication
{
    public sealed record AccessTokenResult(
        string AccessToken,
        DateTime ExpiresAtUtc);
}
