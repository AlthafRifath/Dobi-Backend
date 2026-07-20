using Dobi.Application.Abstractions.Authentication;
using Dobi.Infrastructure.Identity;
using Dobi.Infrastructure.Persistence;
using Dobi.Shared.Exceptions;
using Dobi.Shared.Pagination;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Authentication
{
    public sealed class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly DobiDbContext _dbContext;

        public IdentityService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, DobiDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
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

        public async Task<PagedResult<IdentityUserInfo>> GetUsersAsync(
        PageRequest pageRequest,
        CancellationToken cancellationToken = default)
        {
            return await SearchUsersAsync(
                new IdentityUserSearchRequest(
                    pageRequest.PageNumber,
                    pageRequest.PageSize,
                    pageRequest.SearchTerm,
                    Array.Empty<string>(),
                    null,
                    null,
                    null),
                cancellationToken);
        }

        public async Task<PagedResult<IdentityUserInfo>> SearchUsersAsync(
        IdentityUserSearchRequest request,
        CancellationToken cancellationToken = default)
        {
            var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
            var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;

            var query = _userManager.Users
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.Trim().ToLower();

                query = query.Where(user =>
                    user.FullName.ToLower().Contains(searchTerm) ||
                    (user.UserName != null && user.UserName.ToLower().Contains(searchTerm)) ||
                    (user.Email != null && user.Email.ToLower().Contains(searchTerm)) ||
                    (user.PhoneNumber != null && user.PhoneNumber.ToLower().Contains(searchTerm)));
            }

            if (request.IsActive.HasValue)
            {
                query = query.Where(user => user.IsActive == request.IsActive.Value);
            }

            if (request.BranchId.HasValue)
            {
                var branchId = request.BranchId.Value;

                var assignedBranchUserIds = _dbContext.UserBranchAssignments
                    .AsNoTracking()
                    .Where(x => x.BranchId == branchId)
                    .Select(x => x.UserId);

                query = query.Where(user =>
                    user.DefaultBranchId == branchId ||
                    assignedBranchUserIds.Contains(user.Id));
            }

            if (request.PlantId.HasValue)
            {
                var plantId = request.PlantId.Value;

                var assignedPlantUserIds = _dbContext.UserPlantAssignments
                    .AsNoTracking()
                    .Where(x => x.PlantId == plantId)
                    .Select(x => x.UserId);

                query = query.Where(user =>
                    user.DefaultPlantId == plantId ||
                    assignedPlantUserIds.Contains(user.Id));
            }

            var roleCodes = request.RoleCodes
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim().ToUpperInvariant())
                .Distinct()
                .ToArray();

            if (roleCodes.Length > 0)
            {
                var roleIds = await _dbContext.Set<ApplicationRole>()
                    .AsNoTracking()
                    .Where(role =>
                        (role.NormalizedName != null && roleCodes.Contains(role.NormalizedName)) ||
                        (role.Name != null && roleCodes.Contains(role.Name.ToUpper())))
                    .Select(role => role.Id)
                    .ToArrayAsync(cancellationToken);

                if (roleIds.Length == 0)
                {
                    return new PagedResult<IdentityUserInfo>(
                        Array.Empty<IdentityUserInfo>(),
                        0,
                        pageNumber,
                        pageSize);
                }

                var userIdsInRoles = _dbContext.Set<IdentityUserRole<int>>()
                    .AsNoTracking()
                    .Where(userRole => roleIds.Contains(userRole.RoleId))
                    .Select(userRole => userRole.UserId)
                    .Distinct();

                query = query.Where(user => userIdsInRoles.Contains(user.Id));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var users = await query
                .OrderBy(user => user.FullName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToArrayAsync(cancellationToken);

            var mappedUsers = await MapUsersWithAssignmentsAsync(
                users,
                cancellationToken);

            return new PagedResult<IdentityUserInfo>(
                mappedUsers,
                totalCount,
                pageNumber,
                pageSize);
        }

        public async Task<IdentityUserInfo?> UpdateUserStatusAsync(
            int userId,
            bool isActive,
            CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
            {
                return null;
            }

            user.IsActive = isActive;
            user.UpdatedAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(x => x.Description));
                throw new BadRequestException($"Failed to update user status: {errors}");
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

        public async Task<IReadOnlyCollection<IdentityRoleInfo>> GetAllRolesAsync(CancellationToken cancellationToken = default)
        {
            var roles = await _roleManager.Roles
                .AsNoTracking()
                .OrderBy(x => x.Id)
                .Select(x => new IdentityRoleInfo(
                    x.Id,
                    x.Name ?? string.Empty,
                    ToDisplayName(x.Name ?? string.Empty),
                    x.Description,
                    x.IsActive))
                .ToArrayAsync(cancellationToken);

            return roles;
        }

        private static string ToDisplayName(string roleCode)
        {
            return roleCode switch
            {
                "ADMIN" => "Admin",
                "OUTLET_STAFF" => "Outlet Staff",
                "PLANT_SUPERVISOR" => "Plant Supervisor",
                "DRIVER" => "Driver",
                "MANAGER" => "Manager",
                "OPERATIONS_DIRECTOR" => "Operations Director",
                _ => roleCode.Replace("_", " ")
            };
        }

        private async Task<IReadOnlyCollection<IdentityUserInfo>> MapUsersWithAssignmentsAsync(
        IReadOnlyCollection<ApplicationUser> users,
        CancellationToken cancellationToken)
        {
            if (users.Count == 0)
            {
                return Array.Empty<IdentityUserInfo>();
            }

            var userIds = users
                .Select(x => x.Id)
                .ToArray();

            var roleRows = await (
                from userRole in _dbContext.Set<IdentityUserRole<int>>().AsNoTracking()
                join role in _dbContext.Set<ApplicationRole>().AsNoTracking()
                    on userRole.RoleId equals role.Id
                where userIds.Contains(userRole.UserId)
                select new UserRoleRow(
                    userRole.UserId,
                    role.Id,
                    role.Name ?? string.Empty,
                    ToDisplayName(role.Name ?? string.Empty)))
                .ToListAsync(cancellationToken);

            var branchRows = await (
                from assignment in _dbContext.UserBranchAssignments.AsNoTracking()
                join branch in _dbContext.Branches.AsNoTracking()
                    on assignment.BranchId equals branch.Id
                where userIds.Contains(assignment.UserId)
                select new UserBranchRow(
                    assignment.UserId,
                    branch.Id,
                    branch.BranchName))
                .ToListAsync(cancellationToken);

            var plantRows = await (
                from assignment in _dbContext.UserPlantAssignments.AsNoTracking()
                join plant in _dbContext.Plants.AsNoTracking()
                    on assignment.PlantId equals plant.Id
                where userIds.Contains(assignment.UserId)
                select new UserPlantRow(
                    assignment.UserId,
                    plant.Id,
                    plant.PlantName))
                .ToListAsync(cancellationToken);

            var rolesByUserId = roleRows
                .GroupBy(x => x.UserId)
                .ToDictionary(
                    x => x.Key,
                    x => x
                        .Select(role => new IdentityUserRoleInfo(
                            role.RoleId,
                            role.RoleCode,
                            role.RoleName))
                        .ToArray() as IReadOnlyCollection<IdentityUserRoleInfo>);

            var branchesByUserId = branchRows
                .GroupBy(x => x.UserId)
                .ToDictionary(
                    x => x.Key,
                    x => x
                        .Select(branch => new IdentityUserBranchAssignmentInfo(
                            branch.BranchId,
                            branch.BranchName))
                        .ToArray() as IReadOnlyCollection<IdentityUserBranchAssignmentInfo>);

            var plantsByUserId = plantRows
                .GroupBy(x => x.UserId)
                .ToDictionary(
                    x => x.Key,
                    x => x
                        .Select(plant => new IdentityUserPlantAssignmentInfo(
                            plant.PlantId,
                            plant.PlantName))
                        .ToArray() as IReadOnlyCollection<IdentityUserPlantAssignmentInfo>);

            return users
                .Select(user => new IdentityUserInfo(
                    user.Id,
                    user.FullName,
                    user.UserName ?? string.Empty,
                    user.Email,
                    user.PhoneNumber,
                    user.IsActive,
                    user.DefaultBranchId,
                    user.DefaultPlantId,
                    rolesByUserId.TryGetValue(user.Id, out var roles)
                        ? roles
                        : Array.Empty<IdentityUserRoleInfo>(),
                    branchesByUserId.TryGetValue(user.Id, out var branches)
                        ? branches
                        : Array.Empty<IdentityUserBranchAssignmentInfo>(),
                    plantsByUserId.TryGetValue(user.Id, out var plants)
                        ? plants
                        : Array.Empty<IdentityUserPlantAssignmentInfo>()))
                .ToArray();
        }

        private sealed record UserRoleRow(
            int UserId,
            int RoleId,
            string RoleCode,
            string RoleName);

        private sealed record UserBranchRow(
            int UserId,
            int BranchId,
            string BranchName);

        private sealed record UserPlantRow(
            int UserId,
            int PlantId,
            string PlantName);
    }
}
