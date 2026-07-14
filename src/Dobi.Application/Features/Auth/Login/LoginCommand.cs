using Dobi.Contracts.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Auth.Login
{
    public sealed record LoginCommand(
        string UserNameOrEmail,
        string Password) : IRequest<LoginResponse>;
}
