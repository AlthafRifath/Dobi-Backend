using Dobi.Contracts.Branches;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Branches.UpdateBranchStatus
{
    public sealed record UpdateBranchStatusCommand(
        int BranchId,
        bool IsActive) : IRequest<BranchResponse>;
}
