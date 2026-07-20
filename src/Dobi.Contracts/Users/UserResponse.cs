using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Users
{
    public sealed record UserResponse(
        int UserId,
        string FullName,
        string UserName,
        string? Email,
        string? PhoneNumber,
        bool IsActive,
        int? DefaultBranchId,
        int? DefaultPlantId,
        IReadOnlyCollection<UserRoleResponse>? Roles = null,
        IReadOnlyCollection<UserBranchAssignmentResponse>? BranchAssignments = null,
        IReadOnlyCollection<UserPlantAssignmentResponse>? PlantAssignments = null,
        IReadOnlyCollection<string>? RoleCodes = null,
        IReadOnlyCollection<string>? RoleNames = null);
}
