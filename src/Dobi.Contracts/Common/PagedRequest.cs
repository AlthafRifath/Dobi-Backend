using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Common
{
    public sealed record PagedRequest(
        int PageNumber = 1,
        int PageSize = 10,
        string? SearchTerm = null);
}
