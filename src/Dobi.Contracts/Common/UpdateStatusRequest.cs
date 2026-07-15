using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Common
{
    public sealed record UpdateStatusRequest(
        bool IsActive);
}
