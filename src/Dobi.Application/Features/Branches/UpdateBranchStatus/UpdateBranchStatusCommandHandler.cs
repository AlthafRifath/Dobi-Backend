using Dobi.Application.Abstractions.Persistence;
using Dobi.Application.Abstractions.Services;
using Dobi.Contracts.Branches;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Branches.UpdateBranchStatus
{
    public sealed class UpdateBranchStatusCommandHandler
    : IRequestHandler<UpdateBranchStatusCommand, BranchResponse>
    {
        private readonly IDobiDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public UpdateBranchStatusCommandHandler(
            IDobiDbContext dbContext,
            ICurrentUserService currentUserService,
            IDateTimeProvider dateTimeProvider)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<BranchResponse> Handle(
            UpdateBranchStatusCommand request,
            CancellationToken cancellationToken)
        {
            var branch = await _dbContext.Branches
                .FirstOrDefaultAsync(x => x.Id == request.BranchId, cancellationToken);

            if (branch is null)
            {
                throw new NotFoundException("Branch", request.BranchId);
            }

            branch.IsActive = request.IsActive;
            branch.UpdatedAt = _dateTimeProvider.UtcNow;
            branch.UpdatedByUserId = _currentUserService.UserId;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new BranchResponse(
                branch.Id,
                branch.BranchName,
                branch.Address,
                branch.ContactNo,
                branch.IsActive);
        }
    }
}
