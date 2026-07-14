using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Lookups
{
    public sealed record LookupResponse(
        int Id,
        string Code,
        string Name);
}
