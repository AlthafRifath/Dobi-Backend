using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Services;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Services.UpdateService
{
    public sealed class UpdateServiceCommandHandler
    : IRequestHandler<UpdateServiceCommand, ServiceResponse>
    {
        private readonly IDobiDbContext _dbContext;

        public UpdateServiceCommandHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ServiceResponse> Handle(
            UpdateServiceCommand request,
            CancellationToken cancellationToken)
        {
            var service = await _dbContext.Services
                .FirstOrDefaultAsync(x => x.Id == request.ServiceId, cancellationToken);

            if (service is null)
            {
                throw new NotFoundException("Service", request.ServiceId);
            }

            var serviceCode = request.ServiceCode.Trim().ToUpperInvariant();
            var serviceName = request.ServiceName.Trim();

            var duplicateExists = await _dbContext.Services.AnyAsync(
                x => x.Id != request.ServiceId &&
                     (x.ServiceCode == serviceCode ||
                      x.ServiceName.ToLower() == serviceName.ToLower()),
                cancellationToken);

            if (duplicateExists)
            {
                throw new ConflictException("A service with the same code or name already exists.");
            }

            service.ServiceCode = serviceCode;
            service.ServiceName = serviceName;
            service.Description = string.IsNullOrWhiteSpace(request.Description)
                ? null
                : request.Description.Trim();
            service.IsExpressEligible = request.IsExpressEligible;
            service.IsActive = request.IsActive;

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
