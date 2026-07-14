using Dobi.Contracts.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

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
