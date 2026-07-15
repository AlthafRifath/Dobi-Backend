using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Services;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Services.UpdateServiceStatus
{
    public sealed class UpdateServiceStatusCommandHandler
    : IRequestHandler<UpdateServiceStatusCommand, ServiceResponse>
    {
        private readonly IDobiDbContext _dbContext;

        public UpdateServiceStatusCommandHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ServiceResponse> Handle(
            UpdateServiceStatusCommand request,
            CancellationToken cancellationToken)
        {
            var service = await _dbContext.Services
                .FirstOrDefaultAsync(x => x.Id == request.ServiceId, cancellationToken);

            if (service is null)
            {
                throw new NotFoundException("Service", request.ServiceId);
            }

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
