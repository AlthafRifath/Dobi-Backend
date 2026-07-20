using Dobi.Contracts.Common;
using Dobi.Contracts.Orders;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Orders.GetEligiblePaymentOrders
{
    public sealed record GetEligiblePaymentOrdersQuery(
        int PageNumber,
        int PageSize,
        string? SearchTerm,
        int? BranchId,
        int? CustomerId,
        int? CustomerTypeId,
        IReadOnlyCollection<int> ExcludeOrderIds)
        : IRequest<PagedResponse<EligiblePaymentOrderResponse>>;
}
