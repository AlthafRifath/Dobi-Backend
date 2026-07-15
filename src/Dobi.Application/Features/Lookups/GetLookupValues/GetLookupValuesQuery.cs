using Dobi.Contracts.Lookups;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Lookups.GetLookupValues
{
    public sealed record GetLookupValuesQuery(
        string LookupType) : IRequest<IReadOnlyCollection<LookupResponse>>;
}
