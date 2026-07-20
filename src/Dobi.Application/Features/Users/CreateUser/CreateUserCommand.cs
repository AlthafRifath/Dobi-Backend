using MediatR;
using UserResponse = Dobi.Contracts.Users.UserResponse;

namespace Dobi.Application.Features.Users.CreateUser
{
    public sealed record CreateUserCommand(
        string FullName,
        string UserName,
        string? Email,
        string? PhoneNumber,
        string Password,
        IReadOnlyCollection<string> Roles,
        int? DefaultBranchId,
        int? DefaultPlantId) : IRequest<UserResponse>;
}