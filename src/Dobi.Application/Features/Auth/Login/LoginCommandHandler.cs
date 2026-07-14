using Dobi.Application.Abstractions.Authentication;
using Dobi.Contracts.Auth;
using Dobi.Shared.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Auth.Login
{
    public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IIdentityService _identityService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IRefreshTokenService _refreshTokenService;

        public LoginCommandHandler(
            IIdentityService identityService,
            IJwtTokenService jwtTokenService,
            IRefreshTokenService refreshTokenService)
        {
            _identityService = identityService;
            _jwtTokenService = jwtTokenService;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<LoginResponse> Handle(
            LoginCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _identityService.FindByUserNameOrEmailAsync(
                request.UserNameOrEmail,
                cancellationToken);

            if (user is null)
            {
                throw new UnauthorizedException("Invalid username/email or password.");
            }

            if (!user.IsActive)
            {
                throw new ForbiddenException("This user account is inactive.");
            }

            var isPasswordValid = await _identityService.CheckPasswordAsync(
                user.UserId,
                request.Password,
                cancellationToken);

            if (!isPasswordValid)
            {
                throw new UnauthorizedException("Invalid username/email or password.");
            }

            var roles = await _identityService.GetRolesAsync(
                user.UserId,
                cancellationToken);

            var accessTokenResult = _jwtTokenService.GenerateAccessToken(
                user.UserId,
                user.UserName,
                user.Email,
                roles);

            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            await _refreshTokenService.SaveRefreshTokenAsync(
                user.UserId,
                refreshToken,
                DateTime.UtcNow.AddDays(7),
                cancellationToken);

            return new LoginResponse(
                user.UserId,
                user.FullName,
                user.UserName,
                user.Email,
                roles,
                accessTokenResult.AccessToken,
                refreshToken,
                accessTokenResult.ExpiresAtUtc);
        }
    }
}
