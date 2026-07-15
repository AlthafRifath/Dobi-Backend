using Dobi.Contracts.Common;
using Dobi.Contracts.Orders;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Orders.GetOrders
{
    public sealed record GetOrdersQuery(
        int PageNumber,
        int PageSize,
        string? SearchTerm,
        int? BranchId,
        int? CustomerId,
        int? StatusId,
        int? PaymentStatusId,
        DateOnly? FromDate,
        DateOnly? ToDate) : IRequest<PagedResponse<OrderResponse>>;
}
