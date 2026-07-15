using Dobi.Contracts.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.ServicePrices.UpdateServicePriceStatus
{
    public sealed record UpdateServicePriceStatusCommand(
        int ServicePriceId,
        bool IsActive) : IRequest<ServicePriceResponse>;
}
