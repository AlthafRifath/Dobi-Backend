using Dobi.Infrastructure.Identity;
using Dobi.Shared.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Persistence.Seed
{
    public static class IdentitySeeder
    {
        public static async Task SeedDefaultAdminAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            await EnsureRoleAsync(roleManager, RoleCodes.Admin);
            await EnsureRoleAsync(roleManager, RoleCodes.OutletStaff);
            await EnsureRoleAsync(roleManager, RoleCodes.PlantSupervisor);
            await EnsureRoleAsync(roleManager, RoleCodes.Driver);
            await EnsureRoleAsync(roleManager, RoleCodes.Manager);
            await EnsureRoleAsync(roleManager, RoleCodes.OperationsDirector);

            const string adminUserName = "admin";
            const string adminEmail = "admin@dobi.local";
            const string adminPassword = "Admin@12345";

            var existingAdmin = await userManager.FindByNameAsync(adminUserName);

            if (existingAdmin is not null)
            {
                return;
            }

            var adminUser = new ApplicationUser
            {
                UserName = adminUserName,
                Email = adminEmail,
                FullName = "System Administrator",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var createResult = await userManager.CreateAsync(adminUser, adminPassword);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(x => x.Description));
                throw new InvalidOperationException($"Failed to create default admin user: {errors}");
            }

            await userManager.AddToRoleAsync(adminUser, RoleCodes.Admin);
        }

        private static async Task EnsureRoleAsync(
            RoleManager<ApplicationRole> roleManager,
            string roleCode)
        {
            var roleExists = await roleManager.RoleExistsAsync(roleCode);

            if (roleExists)
            {
                return;
            }

            var role = new ApplicationRole
            {
                Name = roleCode,
                Description = $"{roleCode} role",
                IsActive = true
            };

            var result = await roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(x => x.Description));
                throw new InvalidOperationException($"Failed to create role '{roleCode}': {errors}");
            }
        }
    }
}
