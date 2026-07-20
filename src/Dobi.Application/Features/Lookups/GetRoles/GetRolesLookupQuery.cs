using Dobi.Contracts.Lookups;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Lookups.GetRoles
{
    public sealed record GetRolesLookupQuery() : IRequest<IReadOnlyCollection<RoleLookupResponse>>;
}
