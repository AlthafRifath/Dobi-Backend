using Dobi.Application.Abstractions.Authentication;
using Dobi.Infrastructure.Identity;
using Dobi.Shared.Exceptions;
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

        public async Task<IdentityUserInfo?> FindByUserIdAsync(
            int userId,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

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

        public async Task<bool> UserNameExistsAsync(
            string userName,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByNameAsync(userName);

            return user is not null;
        }

        public async Task<bool> EmailExistsAsync(
            string email,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return user is not null;
        }

        public async Task<IdentityUserInfo> CreateUserAsync(
            CreateIdentityUserRequest request,
            CancellationToken cancellationToken = default)
        {
            var user = new ApplicationUser
            {
                FullName = request.FullName.Trim(),
                UserName = request.UserName.Trim(),
                Email = string.IsNullOrWhiteSpace(request.Email) ? null : request.Email.Trim(),
                PhoneNumber = string.IsNullOrWhiteSpace(request.PhoneNumber) ? null : request.PhoneNumber.Trim(),
                IsActive = true,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                CreatedAt = DateTime.UtcNow,
                DefaultBranchId = request.DefaultBranchId,
                DefaultPlantId = request.DefaultPlantId
            };

            var createResult = await _userManager.CreateAsync(user, request.Password);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(x => x.Description));
                throw new BadRequestException($"Failed to create user: {errors}");
            }

            foreach (var role in request.Roles.Distinct())
            {
                var roleResult = await _userManager.AddToRoleAsync(user, role);

                if (!roleResult.Succeeded)
                {
                    var errors = string.Join(", ", roleResult.Errors.Select(x => x.Description));
                    throw new BadRequestException($"Failed to assign role '{role}': {errors}");
                }
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
    }
}
