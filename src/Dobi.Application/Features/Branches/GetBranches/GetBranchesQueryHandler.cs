using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Branches;
using Dobi.Contracts.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Branches.GetBranches
{
    public sealed class GetBranchesQueryHandler
    : IRequestHandler<GetBranchesQuery, PagedResponse<BranchResponse>>
    {
        private readonly IDobiDbContext _dbContext;

        public GetBranchesQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResponse<BranchResponse>> Handle(
            GetBranchesQuery request,
            CancellationToken cancellationToken)
        {
            var query = _dbContext.Branches.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.Trim().ToLower();

                query = query.Where(x =>
                    x.BranchName.ToLower().Contains(searchTerm) ||
                    (x.Address != null && x.Address.ToLower().Contains(searchTerm)) ||
                    (x.ContactNo != null && x.ContactNo.ToLower().Contains(searchTerm)));
            }

            if (request.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == request.IsActive.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var branches = await query
                .OrderBy(x => x.BranchName)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new BranchResponse(
                    x.Id,
                    x.BranchName,
                    x.Address,
                    x.ContactNo,
                    x.IsActive))
                .ToArrayAsync(cancellationToken);

            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            return new PagedResponse<BranchResponse>(
                branches,
                totalCount,
                request.PageNumber,
                request.PageSize,
                totalPages,
                request.PageNumber > 1,
                request.PageNumber < totalPages);
        }
    }
}
