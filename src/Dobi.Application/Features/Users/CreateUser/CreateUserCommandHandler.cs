using Dobi.Application.Abstractions.Authentication;
using Dobi.Shared.Exceptions;
using MediatR;
using UserResponse = Dobi.Contracts.Users.UserResponse;

namespace Dobi.Application.Features.Users.CreateUser
{
    public sealed class CreateUserCommandHandler
        : IRequestHandler<CreateUserCommand, UserResponse>
    {
        private readonly IIdentityService _identityService;

        public CreateUserCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<UserResponse> Handle(
            CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            if (await _identityService.UserNameExistsAsync(request.UserName, cancellationToken))
            {
                throw new ConflictException("Username already exists.");
            }

            if (!string.IsNullOrWhiteSpace(request.Email) &&
                await _identityService.EmailExistsAsync(request.Email, cancellationToken))
            {
                throw new ConflictException("Email already exists.");
            }

            var createdUser = await _identityService.CreateUserAsync(
                new CreateIdentityUserRequest(
                    request.FullName,
                    request.UserName,
                    request.Email,
                    request.PhoneNumber,
                    request.Password,
                    request.Roles,
                    request.DefaultBranchId,
                    request.DefaultPlantId),
                cancellationToken);

            return UserResponseMapper.Map(createdUser, request.Roles);
        }
    }
}