using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Abstractions.Services
{
    public interface IAuditLogService
    {
        Task LogAsync(
            CreateAuditLogRequest request,
            CancellationToken cancellationToken = default);
    }
}
