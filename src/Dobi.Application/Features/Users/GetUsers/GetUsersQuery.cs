using Dobi.Contracts.Auth;
using Dobi.Contracts.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Users.GetUsers
{
    public sealed record GetUsersQuery(
        int PageNumber,
        int PageSize,
        string? SearchTerm) : IRequest<PagedResponse<UserResponse>>;
}
