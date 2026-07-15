using Dobi.Contracts.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Services.UpdateServiceStatus
{
    public sealed record UpdateServiceStatusCommand(
        int ServiceId,
        bool IsActive) : IRequest<ServiceResponse>;
}
