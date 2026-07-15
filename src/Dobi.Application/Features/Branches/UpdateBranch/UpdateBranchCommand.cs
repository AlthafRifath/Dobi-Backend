using Dobi.Contracts.Branches;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Branches.UpdateBranch
{
    public sealed record UpdateBranchCommand(
        int BranchId,
        string BranchName,
        string? Address,
        string? ContactNo,
        bool IsActive) : IRequest<BranchResponse>;
}
