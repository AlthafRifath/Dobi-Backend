using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Branches;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Branches.GetBranchById
{
    public sealed class GetBranchByIdQueryHandler
    : IRequestHandler<GetBranchByIdQuery, BranchResponse>
    {
        private readonly IDobiDbContext _dbContext;

        public GetBranchByIdQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BranchResponse> Handle(
            GetBranchByIdQuery request,
            CancellationToken cancellationToken)
        {
            var branch = await _dbContext.Branches
                .AsNoTracking()
                .Where(x => x.Id == request.BranchId)
                .Select(x => new BranchResponse(
                    x.Id,
                    x.BranchName,
                    x.Address,
                    x.ContactNo,
                    x.IsActive))
                .FirstOrDefaultAsync(cancellationToken);

            if (branch is null)
            {
                throw new NotFoundException("Branch", request.BranchId);
            }

            return branch;
        }
    }
}
