using Dobi.Application.Abstractions.Authentication;
using Dobi.Contracts.Auth;
using Dobi.Shared.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Users.GetUserById
{
    public sealed class GetUserByIdQueryHandler
    : IRequestHandler<GetUserByIdQuery, UserResponse>
    {
        private readonly IIdentityService _identityService;

        public GetUserByIdQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<UserResponse> Handle(
            GetUserByIdQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _identityService.FindByUserIdAsync(
                request.UserId,
                cancellationToken);

            if (user is null)
            {
                throw new NotFoundException("User", request.UserId);
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
