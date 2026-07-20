using Dobi.Application.Abstractions.Authentication;
using Dobi.Shared.Exceptions;
using MediatR;
using UserResponse = Dobi.Contracts.Users.UserResponse;

namespace Dobi.Application.Features.Users.UpdateUserStatus
{
    public sealed class UpdateUserStatusCommandHandler
        : IRequestHandler<UpdateUserStatusCommand, UserResponse>
    {
        private readonly IIdentityService _identityService;

        public UpdateUserStatusCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<UserResponse> Handle(
            UpdateUserStatusCommand request,
            CancellationToken cancellationToken)
        {
            var updatedUser = await _identityService.UpdateUserStatusAsync(
                request.UserId,
                request.IsActive,
                cancellationToken);

            if (updatedUser is null)
            {
                throw new NotFoundException("User", request.UserId);
            }

            var roles = await _identityService.GetRolesAsync(
                request.UserId,
                cancellationToken);

            return UserResponseMapper.Map(updatedUser, roles);
        }
    }
}