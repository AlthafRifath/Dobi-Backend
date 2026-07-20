using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Users
{
    public sealed record UserBranchAssignmentResponse(
        int BranchId,
        string BranchName);
}
