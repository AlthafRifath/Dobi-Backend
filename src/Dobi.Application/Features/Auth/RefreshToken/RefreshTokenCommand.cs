using Dobi.Contracts.Auth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Auth.RefreshToken
{
    public sealed record RefreshTokenCommand(
        int UserId,
        string RefreshToken) : IRequest<LoginResponse>;
}
