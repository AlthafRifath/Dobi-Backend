using Dobi.Contracts.Common;
using Dobi.Contracts.Orders;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Orders.GetEligibleRefundOrders
{
    public sealed record GetEligibleRefundOrdersQuery(
        int PageNumber,
        int PageSize,
        string? SearchTerm,
        int? CustomerId,
        int? BranchId,
        int? CustomerTypeId,
        IReadOnlyCollection<int> ExcludeOrderIds)
        : IRequest<PagedResponse<EligibleRefundOrderResponse>>;
}
