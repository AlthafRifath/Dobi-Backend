using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Services;
using Dobi.Domain.Services;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Services.CreateService
{
    public sealed class CreateServiceCommandHandler
    : IRequestHandler<CreateServiceCommand, ServiceResponse>
    {
        private readonly IDobiDbContext _dbContext;

        public CreateServiceCommandHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ServiceResponse> Handle(
            CreateServiceCommand request,
            CancellationToken cancellationToken)
        {
            var serviceCode = request.ServiceCode.Trim().ToUpperInvariant();
            var serviceName = request.ServiceName.Trim();

            var duplicateExists = await _dbContext.Services.AnyAsync(
                x => x.ServiceCode == serviceCode ||
                     x.ServiceName.ToLower() == serviceName.ToLower(),
                cancellationToken);

            if (duplicateExists)
            {
                throw new ConflictException("A service with the same code or name already exists.");
            }

            var service = new Service
            {
                ServiceCode = serviceCode,
                ServiceName = serviceName,
                Description = string.IsNullOrWhiteSpace(request.Description)
                    ? null
                    : request.Description.Trim(),
                IsExpressEligible = request.IsExpressEligible,
                IsActive = request.IsActive
            };

            _dbContext.Services.Add(service);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new ServiceResponse(
                service.Id,
                service.ServiceCode,
                service.ServiceName,
                service.Description,
                service.IsExpressEligible,
                service.IsActive);
        }
    }
}
