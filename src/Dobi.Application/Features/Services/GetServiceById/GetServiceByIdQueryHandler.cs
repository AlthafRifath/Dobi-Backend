using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.Services;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Services.GetServiceById
{
    public sealed class GetServiceByIdQueryHandler
    : IRequestHandler<GetServiceByIdQuery, ServiceResponse>
    {
        private readonly IDobiDbContext _dbContext;

        public GetServiceByIdQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ServiceResponse> Handle(
            GetServiceByIdQuery request,
            CancellationToken cancellationToken)
        {
            var service = await _dbContext.Services
                .AsNoTracking()
                .Where(x => x.Id == request.ServiceId)
                .Select(x => new ServiceResponse(
                    x.Id,
                    x.ServiceCode,
                    x.ServiceName,
                    x.Description,
                    x.IsExpressEligible,
                    x.IsActive))
                .FirstOrDefaultAsync(cancellationToken);

            if (service is null)
            {
                throw new NotFoundException("Service", request.ServiceId);
            }

            return service;
        }
    }
}
