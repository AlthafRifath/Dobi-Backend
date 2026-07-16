using Dobi.Application.Abstractions.Persistence;
using Dobi.Contracts.AuditLogs;
using Dobi.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.AuditLogs.GetAuditLogById
{
    public sealed class GetAuditLogByIdQueryHandler
    : IRequestHandler<GetAuditLogByIdQuery, AuditLogResponse>
    {
        private readonly IDobiDbContext _dbContext;

        public GetAuditLogByIdQueryHandler(IDobiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AuditLogResponse> Handle(
            GetAuditLogByIdQuery request,
            CancellationToken cancellationToken)
        {
            var auditLog = await _dbContext.AuditLogs
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.AuditLogId, cancellationToken);

            if (auditLog is null)
            {
                throw new NotFoundException("Audit log", request.AuditLogId);
            }

            return AuditLogResponseMapper.Map(auditLog);
        }
    }
}
