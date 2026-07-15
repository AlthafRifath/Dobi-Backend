using Dobi.Contracts.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Users.UpdateUserStatus
{
    public sealed record UpdateUserStatusCommand(
        int UserId,
        bool IsActive) : IRequest<UserResponse>;
}
