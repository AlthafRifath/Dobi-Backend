using Dobi.Contracts.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.ServicePrices.GetServicePriceById
{
    public sealed record GetServicePriceByIdQuery(
        int ServicePriceId) : IRequest<ServicePriceResponse>;
}
