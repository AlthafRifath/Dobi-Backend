using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Abstractions.Authentication
{
    public sealed record IdentityUserBranchAssignmentInfo(
        int BranchId,
        string BranchName);
}
