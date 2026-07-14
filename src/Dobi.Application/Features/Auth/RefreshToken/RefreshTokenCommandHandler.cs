using Dobi.Application.Abstractions.Authentication;
using Dobi.Contracts.Auth;
using Dobi.Shared.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Auth.RefreshToken
{
    public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginResponse>
    {
        private readonly IIdentityService _identityService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IRefreshTokenService _refreshTokenService;

        public RefreshTokenCommandHandler(
            IIdentityService identityService,
            IJwtTokenService jwtTokenService,
            IRefreshTokenService refreshTokenService)
        {
            _identityService = identityService;
            _jwtTokenService = jwtTokenService;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<LoginResponse> Handle(
            RefreshTokenCommand request,
            CancellationToken cancellationToken)
        {
            var isRefreshTokenValid = await _refreshTokenService.ValidateRefreshTokenAsync(
                request.UserId,
                request.RefreshToken,
                cancellationToken);

            if (!isRefreshTokenValid)
            {
                throw new UnauthorizedException("Invalid or expired refresh token.");
            }

            var user = await _identityService.FindByUserIdAsync(
                request.UserId,
                cancellationToken);

            if (user is null)
            {
                throw new UnauthorizedException("User was not found.");
            }

            if (!user.IsActive)
            {
                throw new ForbiddenException("This user account is inactive.");
            }

            var roles = await _identityService.GetRolesAsync(
                user.UserId,
                cancellationToken);

            var accessTokenResult = _jwtTokenService.GenerateAccessToken(
                user.UserId,
                user.UserName,
                user.Email,
                roles);

            var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

            await _refreshTokenService.SaveRefreshTokenAsync(
                user.UserId,
                newRefreshToken,
                DateTime.UtcNow.AddDays(7),
                cancellationToken);

            return new LoginResponse(
                user.UserId,
                user.FullName,
                user.UserName,
                user.Email,
                roles,
                accessTokenResult.AccessToken,
                newRefreshToken,
                accessTokenResult.ExpiresAtUtc);
        }
    }
}
