using Dobi.Application.Abstractions.Persistence;
using Dobi.Application.Abstractions.Services;
using Dobi.Domain.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Infrastructure.Auditing
{
    public sealed class AuditLogService : IAuditLogService
    {
        private readonly IDobiDbContext _dbContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public AuditLogService(
            IDobiDbContext dbContext,
            IDateTimeProvider dateTimeProvider)
        {
            _dbContext = dbContext;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task LogAsync(
            CreateAuditLogRequest request,
            CancellationToken cancellationToken = default)
        {
            var auditLog = new AuditLog
            {
                EntityName = request.EntityName,
                EntityId = request.EntityId,
                Action = request.Action,
                OldValues = request.OldValues,
                NewValues = request.NewValues,
                PerformedByUserId = request.PerformedByUserId,
                PerformedAt = _dateTimeProvider.UtcNow,
                IpAddress = request.IpAddress,
                UserAgent = request.UserAgent
            };

            _dbContext.AuditLogs.Add(auditLog);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
