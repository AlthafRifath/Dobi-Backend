using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Shared.Pagination
{
    public sealed class PagedResult<T>
    {
        public PagedResult(
            IReadOnlyCollection<T> items,
            int totalCount,
            int pageNumber,
            int pageSize)
        {
            Items = items;
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public IReadOnlyCollection<T> Items { get; }

        public int TotalCount { get; }

        public int PageNumber { get; }

        public int PageSize { get; }

        public int TotalPages => PageSize == 0
            ? 0
            : (int)Math.Ceiling(TotalCount / (double)PageSize);

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;
    }
}
