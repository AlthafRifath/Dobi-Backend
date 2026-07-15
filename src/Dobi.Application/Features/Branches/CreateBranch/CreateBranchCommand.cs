using Dobi.Contracts.Branches;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Branches.CreateBranch
{
    public sealed record CreateBranchCommand(
        string BranchName,
        string? Address,
        string? ContactNo,
        bool IsActive) : IRequest<BranchResponse>;
}
