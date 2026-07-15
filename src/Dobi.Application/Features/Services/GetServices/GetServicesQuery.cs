using Dobi.Contracts.Common;
using Dobi.Contracts.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Services.GetServices
{
    public sealed record GetServicesQuery(
        int PageNumber,
        int PageSize,
        string? SearchTerm,
        bool? IsActive) : IRequest<PagedResponse<ServiceResponse>>;
}
