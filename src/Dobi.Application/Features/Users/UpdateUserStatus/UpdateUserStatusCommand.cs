using MediatR;
using UserResponse = Dobi.Contracts.Users.UserResponse;

namespace Dobi.Application.Features.Users.UpdateUserStatus
{
    public sealed record UpdateUserStatusCommand(
        int UserId,
        bool IsActive) : IRequest<UserResponse>;
}