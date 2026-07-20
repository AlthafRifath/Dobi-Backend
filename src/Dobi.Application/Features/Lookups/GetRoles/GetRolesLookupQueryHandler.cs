using Dobi.Application.Abstractions.Authentication;
using Dobi.Contracts.Lookups;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Application.Features.Lookups.GetRoles
{
    public sealed class GetRolesLookupQueryHandler : IRequestHandler<GetRolesLookupQuery, IReadOnlyCollection<RoleLookupResponse>>
    {
        private readonly IIdentityService _identityService;

        public GetRolesLookupQueryHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<IReadOnlyCollection<RoleLookupResponse>> Handle(
            GetRolesLookupQuery request,
            CancellationToken cancellationToken)
        {
            var roles = await _identityService.GetAllRolesAsync(cancellationToken);

            return roles
                .Select(x => new RoleLookupResponse(
                    x.Id,
                    x.Code,
                    x.Name,
                    x.Description,
                    x.IsActive))
                .ToArray();
        }
    }
}
