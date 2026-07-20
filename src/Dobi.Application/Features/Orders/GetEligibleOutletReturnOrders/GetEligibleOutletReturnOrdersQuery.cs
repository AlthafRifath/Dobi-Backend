using Dobi.Contracts.Common;
using Dobi.Contracts.Orders;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Orders.GetEligibleOutletReturnOrders
{
    public sealed record GetEligibleOutletReturnOrdersQuery(
        int PageNumber,
        int PageSize,
        string? SearchTerm,
        int FromPlantId,
        int ToBranchId,
        IReadOnlyCollection<int> ExcludeOrderIds)
        : IRequest<PagedResponse<EligibleOutletReturnOrderResponse>>;
}
