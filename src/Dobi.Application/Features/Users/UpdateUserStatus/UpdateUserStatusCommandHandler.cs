using Dobi.Application.Abstractions.Authentication;
using Dobi.Application.Abstractions.Services;
using Dobi.Contracts.Auth;
using Dobi.Shared.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Users.UpdateUserStatus
{
    public sealed class UpdateUserStatusCommandHandler
    : IRequestHandler<UpdateUserStatusCommand, UserResponse>
    {
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _currentUserService;

        public UpdateUserStatusCommandHandler(
            IIdentityService identityService,
            ICurrentUserService currentUserService)
        {
            _identityService = identityService;
            _currentUserService = currentUserService;
        }

        public async Task<UserResponse> Handle(
            UpdateUserStatusCommand request,
            CancellationToken cancellationToken)
        {
            if (_currentUserService.UserId == request.UserId && !request.IsActive)
            {
                throw new ConflictException("You cannot deactivate your own account.");
            }

            var updatedUser = await _identityService.UpdateUserStatusAsync(
                request.UserId,
                request.IsActive,
                cancellationToken);

            if (updatedUser is null)
            {
                throw new NotFoundException("User", request.UserId);
            }

            var roles = await _identityService.GetRolesAsync(
                updatedUser.UserId,
                cancellationToken);

            return new UserResponse(
                updatedUser.UserId,
                updatedUser.FullName,
                updatedUser.UserName,
                updatedUser.Email,
                updatedUser.PhoneNumber,
                updatedUser.IsActive,
                roles,
                updatedUser.DefaultBranchId,
                updatedUser.DefaultPlantId);
        }
    }
}
