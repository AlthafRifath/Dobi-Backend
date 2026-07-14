using Dobi.Application.Abstractions.Authentication;
using Dobi.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Authentication
{
    public sealed class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityUserInfo?> FindByUserNameOrEmailAsync(
            string userNameOrEmail,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByNameAsync(userNameOrEmail)
                ?? await _userManager.FindByEmailAsync(userNameOrEmail);

            if (user is null)
            {
                return null;
            }

            return new IdentityUserInfo(
                user.Id,
                user.FullName,
                user.UserName ?? string.Empty,
                user.Email,
                user.PhoneNumber,
                user.IsActive,
                user.DefaultBranchId,
                user.DefaultPlantId);
        }

        public async Task<bool> CheckPasswordAsync(
            int userId,
            string password,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
            {
                return false;
            }

            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IReadOnlyCollection<string>> GetRolesAsync(
            int userId,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
            {
                return Array.Empty<string>();
            }

            var roles = await _userManager.GetRolesAsync(user);

            return roles.ToArray();
        }
    }
}
