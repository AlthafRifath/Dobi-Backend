using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Abstractions.Authentication
{
    public interface IIdentityService
    {
        Task<IdentityUserInfo?> FindByUserNameOrEmailAsync(
            string userNameOrEmail,
            CancellationToken cancellationToken = default);

        Task<bool> CheckPasswordAsync(
            int userId,
            string password,
            CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<string>> GetRolesAsync(
            int userId,
            CancellationToken cancellationToken = default);
    }
}
