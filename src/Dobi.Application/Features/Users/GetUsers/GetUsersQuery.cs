using Dobi.Contracts.Auth;
using Dobi.Contracts.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using UserResponse = Dobi.Contracts.Users.UserResponse;

namespace Dobi.Application.Features.Users.GetUsers
{
    public sealed record GetUsersQuery(
        int PageNumber,
        int PageSize,
        string? SearchTerm,
        IReadOnlyCollection<string> RoleCodes,
        bool? IsActive,
        int? BranchId,
        int? PlantId)
        : IRequest<PagedResponse<UserResponse>>;
}
