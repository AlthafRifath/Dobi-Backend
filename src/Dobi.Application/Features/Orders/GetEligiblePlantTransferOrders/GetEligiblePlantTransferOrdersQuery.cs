using Dobi.Contracts.Common;
using Dobi.Contracts.Orders;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Orders.GetEligiblePlantTransferOrders
{
    public sealed record GetEligiblePlantTransferOrdersQuery(
        int PageNumber,
        int PageSize,
        string? SearchTerm,
        int FromBranchId,
        int ToPlantId,
        int? CustomerTypeId,
        IReadOnlyCollection<int> ExcludeOrderIds)
        : IRequest<PagedResponse<EligiblePlantTransferOrderResponse>>;
}
