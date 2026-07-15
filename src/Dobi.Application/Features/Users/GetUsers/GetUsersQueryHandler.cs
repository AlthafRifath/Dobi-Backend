using Dobi.Application.Abstractions.Authentication;
using Dobi.Contracts.Auth;
using Dobi.Contracts.Common;
using Dobi.Shared.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Users.GetUsers
{
    public sealed class GetUsersQueryHandler
    : IRequestHandler<GetUsersQuery, PagedResponse<UserResponse>>
    {
        private readonly IIdentityService _identityService;

        public GetUsersQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<PagedResponse<UserResponse>> Handle(
            GetUsersQuery request,
            CancellationToken cancellationToken)
        {
            var pageRequest = new PageRequest
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                SearchTerm = request.SearchTerm
            };

            var pagedUsers = await _identityService.GetUsersAsync(
                pageRequest,
                cancellationToken);

            var users = new List<UserResponse>();

            foreach (var user in pagedUsers.Items)
            {
                var roles = await _identityService.GetRolesAsync(
                    user.UserId,
                    cancellationToken);

                users.Add(new UserResponse(
                    user.UserId,
                    user.FullName,
                    user.UserName,
                    user.Email,
                    user.PhoneNumber,
                    user.IsActive,
                    roles,
                    user.DefaultBranchId,
                    user.DefaultPlantId));
            }

            return new PagedResponse<UserResponse>(
                users,
                pagedUsers.TotalCount,
                pagedUsers.PageNumber,
                pagedUsers.PageSize,
                pagedUsers.TotalPages,
                pagedUsers.HasPreviousPage,
                pagedUsers.HasNextPage);
        }
    }
}
