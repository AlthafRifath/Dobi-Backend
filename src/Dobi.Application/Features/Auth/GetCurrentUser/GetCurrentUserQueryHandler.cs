using Dobi.Application.Abstractions.Authentication;
using Dobi.Application.Abstractions.Services;
using Dobi.Contracts.Auth;
using Dobi.Shared.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Auth.GetCurrentUser
{
    public sealed class GetCurrentUserQueryHandler
    : IRequestHandler<GetCurrentUserQuery, UserResponse>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;

        public GetCurrentUserQueryHandler(
            ICurrentUserService currentUserService,
            IIdentityService identityService)
        {
            _currentUserService = currentUserService;
            _identityService = identityService;
        }

        public async Task<UserResponse> Handle(
            GetCurrentUserQuery request,
            CancellationToken cancellationToken)
        {
            if (!_currentUserService.IsAuthenticated || _currentUserService.UserId is null)
            {
                throw new UnauthorizedException("User is not authenticated.");
            }

            var user = await _identityService.FindByUserIdAsync(
                _currentUserService.UserId.Value,
                cancellationToken);

            if (user is null)
            {
                throw new UnauthorizedException("Authenticated user was not found.");
            }

            var roles = await _identityService.GetRolesAsync(
                user.UserId,
                cancellationToken);

            return new UserResponse(
                user.UserId,
                user.FullName,
                user.UserName,
                user.Email,
                user.PhoneNumber,
                user.IsActive,
                roles,
                user.DefaultBranchId,
                user.DefaultPlantId);
        }
    }
}
