using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Branches
{
    public sealed record BranchResponse(
        int BranchId,
        string BranchName,
        string? Address,
        string? ContactNo,
        bool IsActive);
}
