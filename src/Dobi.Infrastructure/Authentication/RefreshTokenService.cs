using Dobi.Application.Abstractions.Authentication;
using Dobi.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Authentication
{
    public sealed class RefreshTokenService : IRefreshTokenService
    {
        private const string LoginProvider = "Dobi";
        private const string RefreshTokenName = "RefreshToken";
        private const string RefreshTokenExpiryName = "RefreshTokenExpiresAtUtc";

        private readonly UserManager<ApplicationUser> _userManager;

        public RefreshTokenService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task SaveRefreshTokenAsync(
            int userId,
            string refreshToken,
            DateTime expiresAtUtc,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
            {
                throw new InvalidOperationException("User was not found while saving refresh token.");
            }

            await _userManager.SetAuthenticationTokenAsync(
                user,
                LoginProvider,
                RefreshTokenName,
                refreshToken);

            await _userManager.SetAuthenticationTokenAsync(
                user,
                LoginProvider,
                RefreshTokenExpiryName,
                expiresAtUtc.ToString("O"));
        }

        public async Task<bool> ValidateRefreshTokenAsync(
            int userId,
            string refreshToken,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
            {
                return false;
            }

            var storedRefreshToken = await _userManager.GetAuthenticationTokenAsync(
                user,
                LoginProvider,
                RefreshTokenName);

            var storedExpiry = await _userManager.GetAuthenticationTokenAsync(
                user,
                LoginProvider,
                RefreshTokenExpiryName);

            if (string.IsNullOrWhiteSpace(storedRefreshToken) ||
                string.IsNullOrWhiteSpace(storedExpiry))
            {
                return false;
            }

            if (!DateTime.TryParse(storedExpiry, out var expiresAtUtc))
            {
                return false;
            }

            if (expiresAtUtc < DateTime.UtcNow)
            {
                return false;
            }

            return storedRefreshToken == refreshToken;
        }
    }
}
