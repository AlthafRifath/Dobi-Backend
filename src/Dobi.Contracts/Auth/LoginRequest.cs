using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Auth
{
    public sealed record LoginRequest(
        string UserNameOrEmail,
        string Password);
}
