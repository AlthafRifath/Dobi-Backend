using Dobi.Contracts.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Auth.GetCurrentUser
{
    public sealed record GetCurrentUserQuery : IRequest<UserResponse>;
}
