using Dobi.Application.Abstractions.Authentication;
using Dobi.Contracts.Common;
using MediatR;
using UserResponse = Dobi.Contracts.Users.UserResponse;

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
            var pagedUsers = await _identityService.SearchUsersAsync(
                new IdentityUserSearchRequest(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchTerm,
                    request.RoleCodes,
                    request.IsActive,
                    request.BranchId,
                    request.PlantId),
                cancellationToken);

            var items = pagedUsers.Items
                .Select(user => UserResponseMapper.Map(user))
                .ToArray();

            var totalPages = (int)Math.Ceiling(
                pagedUsers.TotalCount / (double)request.PageSize);

            return new PagedResponse<UserResponse>(
                items,
                pagedUsers.TotalCount,
                request.PageNumber,
                request.PageSize,
                totalPages,
                request.PageNumber > 1,
                request.PageNumber < totalPages);
        }
    }
}