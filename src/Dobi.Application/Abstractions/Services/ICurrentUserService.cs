using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Abstractions.Services
{
    public interface ICurrentUserService
    {
        int? UserId { get; }

        string? UserName { get; }

        string? Email { get; }

        IReadOnlyCollection<string> Roles { get; }

        bool IsAuthenticated { get; }
    }
}
