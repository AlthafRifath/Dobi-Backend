using Dobi.Application.Abstractions.Persistence;
using Dobi.Application.Abstractions.Services;
using Dobi.Contracts.Branches;
using Dobi.Domain.Branches;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Branches.CreateBranch
{
    public sealed class CreateBranchCommandHandler
    : IRequestHandler<CreateBranchCommand, BranchResponse>
    {
        private readonly IDobiDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CreateBranchCommandHandler(
            IDobiDbContext dbContext,
            ICurrentUserService currentUserService,
            IDateTimeProvider dateTimeProvider)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<BranchResponse> Handle(
            CreateBranchCommand request,
            CancellationToken cancellationToken)
        {
            var branchName = request.BranchName.Trim();

            var exists = await _dbContext.Branches.AnyAsync(
                x => x.BranchName.ToLower() == branchName.ToLower(),
                cancellationToken);

            if (exists)
            {
                throw new ConflictException("A branch with the same name already exists.");
            }

            var branch = new Branch
            {
                BranchName = branchName,
                Address = string.IsNullOrWhiteSpace(request.Address) ? null : request.Address.Trim(),
                ContactNo = string.IsNullOrWhiteSpace(request.ContactNo) ? null : request.ContactNo.Trim(),
                IsActive = request.IsActive,
                CreatedAt = _dateTimeProvider.UtcNow,
                CreatedByUserId = _currentUserService.UserId
            };

            _dbContext.Branches.Add(branch);

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
