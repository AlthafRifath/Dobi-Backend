using Dobi.Contracts.Common;
using Dobi.Contracts.Orders;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Orders.GetEligiblePlantProcessingOrders
{
    public sealed record GetEligiblePlantProcessingOrdersQuery(
        int PageNumber,
        int PageSize,
        string? SearchTerm,
        int PlantId,
        int? CustomerId,
        int? BranchId,
        int? CustomerTypeId,
        IReadOnlyCollection<int> ExcludeOrderIds)
        : IRequest<PagedResponse<EligiblePlantProcessingOrderResponse>>;
}
