using Dobi.Contracts.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Services.GetServiceById
{
    public sealed record GetServiceByIdQuery(
        int ServiceId) : IRequest<ServiceResponse>;
}
