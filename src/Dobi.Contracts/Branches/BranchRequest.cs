using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Branches
{
    public sealed record BranchRequest(
        string BranchName,
        string? Address,
        string? ContactNo,
        bool IsActive = true);
}
