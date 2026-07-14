using Dobi.Application.Abstractions.Authentication;
using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Auth;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Users.CreateUser
{
    public sealed class CreateUserCommandHandler
    : IRequestHandler<CreateUserCommand, UserResponse>
    {
        private readonly IIdentityService _identityService;
        private readonly IDobiDbContext _dbContext;

        public CreateUserCommandHandler(
            IIdentityService identityService,
            IDobiDbContext dbContext)
        {
            _identityService = identityService;
            _dbContext = dbContext;
        }

        public async Task<UserResponse> Handle(
            CreateUserCommand request,
            CancellationToken cancellationToken)
        {
            var normalizedRoles = request.Roles
                .Select(role => role.Trim().ToUpperInvariant())
                .Distinct()
                .ToArray();

            var userNameExists = await _identityService.UserNameExistsAsync(
                request.UserName,
                cancellationToken);

            if (userNameExists)
            {
                throw new ConflictException("Username already exists.");
            }

            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                var emailExists = await _identityService.EmailExistsAsync(
                    request.Email,
                    cancellationToken);

                if (emailExists)
                {
                    throw new ConflictException("Email already exists.");
                }
            }

            if (request.DefaultBranchId.HasValue)
            {
                var branchExists = await _dbContext.Branches.AnyAsync(
                    x => x.Id == request.DefaultBranchId.Value,
                    cancellationToken);

                if (!branchExists)
                {
                    throw new NotFoundException("Branch", request.DefaultBranchId.Value);
                }
            }

            if (request.DefaultPlantId.HasValue)
            {
                var plantExists = await _dbContext.Plants.AnyAsync(
                    x => x.Id == request.DefaultPlantId.Value,
                    cancellationToken);

                if (!plantExists)
                {
                    throw new NotFoundException("Plant", request.DefaultPlantId.Value);
                }
            }

            var user = await _identityService.CreateUserAsync(
                new CreateIdentityUserRequest(
                    request.FullName,
                    request.UserName,
                    request.Email,
                    request.PhoneNumber,
                    request.Password,
                    normalizedRoles,
                    request.DefaultBranchId,
                    request.DefaultPlantId),
                cancellationToken);

            var roles = await _identityService.GetRolesAsync(
                user.UserId,
                cancellationToken);

            return new UserResponse(
                user.UserId,
                user.FullName,
                user.UserName,
                user.Email,
                user.PhoneNumber,
                user.IsActive,
                roles,
                user.DefaultBranchId,
                user.DefaultPlantId);
        }
    }
}
