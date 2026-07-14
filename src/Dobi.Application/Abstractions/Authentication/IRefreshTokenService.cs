using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Abstractions.Authentication
{
    public interface IRefreshTokenService
    {
        Task SaveRefreshTokenAsync(
            int userId,
            string refreshToken,
            DateTime expiresAtUtc,
            CancellationToken cancellationToken = default);

        Task<bool> ValidateRefreshTokenAsync(
            int userId,
            string refreshToken,
            CancellationToken cancellationToken = default);
    }
}
