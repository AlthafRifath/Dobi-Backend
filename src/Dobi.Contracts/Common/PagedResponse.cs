using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Common
{
    public sealed record PagedResponse<T>(
        IReadOnlyCollection<T> Items,
        int TotalCount,
        int PageNumber,
        int PageSize,
        int TotalPages,
        bool HasPreviousPage,
        bool HasNextPage);
}
