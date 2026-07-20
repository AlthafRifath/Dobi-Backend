using Dobi.Application.Abstractions.Authentication;
using Dobi.Contracts.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Users
{
    internal static class UserResponseMapper
    {
        public static UserResponse Map(
            IdentityUserInfo user,
            IReadOnlyCollection<string>? fallbackRoleNames = null)
        {
            var roles = user.Roles ?? Array.Empty<IdentityUserRoleInfo>();

            if (roles.Count == 0 && fallbackRoleNames is not null)
            {
                roles = fallbackRoleNames
                    .Select(role => new IdentityUserRoleInfo(
                        0,
                        role,
                        ToDisplayName(role)))
                    .ToArray();
            }

            var roleResponses = roles
                .Select(role => new UserRoleResponse(
                    role.Id,
                    role.Code,
                    role.Name))
                .ToArray();

            var branchAssignments = (user.BranchAssignments ?? Array.Empty<IdentityUserBranchAssignmentInfo>())
                .Select(branch => new UserBranchAssignmentResponse(
                    branch.BranchId,
                    branch.BranchName))
                .ToArray();

            var plantAssignments = (user.PlantAssignments ?? Array.Empty<IdentityUserPlantAssignmentInfo>())
                .Select(plant => new UserPlantAssignmentResponse(
                    plant.PlantId,
                    plant.PlantName))
                .ToArray();

            return new UserResponse(
                user.UserId,
                user.FullName,
                user.UserName,
                user.Email,
                user.PhoneNumber,
                user.IsActive,
                user.DefaultBranchId,
                user.DefaultPlantId,
                roleResponses,
                branchAssignments,
                plantAssignments,
                roleResponses.Select(x => x.Code).ToArray(),
                roleResponses.Select(x => x.Name).ToArray());
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
    }
}
