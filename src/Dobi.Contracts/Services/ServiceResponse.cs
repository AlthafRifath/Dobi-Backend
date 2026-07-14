using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Services
{
    public sealed record ServiceResponse(
        int ServiceId,
        string ServiceCode,
        string ServiceName,
        string? Description,
        bool IsExpressEligible,
        bool IsActive);
}
