using MediatR;
using UserResponse = Dobi.Contracts.Users.UserResponse;

namespace Dobi.Application.Features.Users.GetUserById
{
    public sealed record GetUserByIdQuery(
        int UserId) : IRequest<UserResponse>;
}