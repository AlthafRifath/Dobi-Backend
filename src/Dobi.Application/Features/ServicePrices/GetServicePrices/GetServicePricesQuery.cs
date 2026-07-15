using Dobi.Contracts.Common;
using Dobi.Contracts.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.ServicePrices.GetServicePrices
{
    public sealed record GetServicePricesQuery(
        int PageNumber,
        int PageSize,
        int? ServiceId,
        int? ItemCategoryId,
        int? PricingTypeId,
        bool? IsActive) : IRequest<PagedResponse<ServicePriceResponse>>;
}
