using Dobi.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Abstractions.Authentication
{
    public interface IIdentityService
    {
        Task<IdentityUserInfo?> FindByUserIdAsync(
            int userId,
            CancellationToken cancellationToken = default);

        Task<IdentityUserInfo?> FindByUserNameOrEmailAsync(
            string userNameOrEmail,
            CancellationToken cancellationToken = default);

        Task<bool> UserNameExistsAsync(
            string userName,
            CancellationToken cancellationToken = default);

        Task<bool> EmailExistsAsync(
            string email,
            CancellationToken cancellationToken = default);

        Task<bool> CheckPasswordAsync(
            int userId,
            string password,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<string>> GetRolesAsync(
            int userId,
            CancellationToken cancellationToken = default);

        Task<IdentityUserInfo> CreateUserAsync(
            CreateIdentityUserRequest request,
            CancellationToken cancellationToken = default);

        Task<PagedResult<IdentityUserInfo>> GetUsersAsync(
            PageRequest pageRequest,
            CancellationToken cancellationToken = default);

        Task<IdentityUserInfo?> UpdateUserStatusAsync(
            int userId,
            bool isActive,
            CancellationToken cancellationToken = default);
    }
}
