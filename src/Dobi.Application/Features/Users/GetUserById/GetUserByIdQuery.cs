using Dobi.Contracts.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Users.GetUserById
{
    public sealed record GetUserByIdQuery(
        int UserId) : IRequest<UserResponse>;
}
