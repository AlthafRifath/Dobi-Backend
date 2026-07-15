using Dobi.Contracts.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Services.CreateService
{
    public sealed record CreateServiceCommand(
        string ServiceCode,
        string ServiceName,
        string? Description,
        bool IsExpressEligible,
        bool IsActive) : IRequest<ServiceResponse>;
}
