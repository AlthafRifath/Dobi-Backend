using Dobi.Contracts.Collections;
using Dobi.Contracts.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Collections.GetCollections
{
    public sealed record GetCollectionsQuery(
        int PageNumber,
        int PageSize,
        string? SearchTerm,
        int? CollectionModeId,
        DateOnly? FromDate,
        DateOnly? ToDate) : IRequest<PagedResponse<OrderCollectionResponse>>;
}
