using Dobi.Contracts.Branches;
using Dobi.Contracts.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Branches.GetBranches
{
    public sealed record GetBranchesQuery(
        int PageNumber,
        int PageSize,
        string? SearchTerm,
        bool? IsActive) : IRequest<PagedResponse<BranchResponse>>;
}
