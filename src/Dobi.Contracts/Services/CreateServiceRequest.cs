using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Services
{
    public sealed record CreateServiceRequest(
        string ServiceCode,
        string ServiceName,
        string? Description,
        bool IsExpressEligible,
        bool IsActive = true);
}
