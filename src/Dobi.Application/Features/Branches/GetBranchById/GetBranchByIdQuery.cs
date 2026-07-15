using Dobi.Contracts.Branches;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Branches.GetBranchById
{
    public sealed record GetBranchByIdQuery(
        int BranchId) : IRequest<BranchResponse>;
}
