using Dobi.Application.Abstractions.Services;
using System.Security.Claims;

namespace Dobi.Api.Context
{
    public sealed class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? UserId
        {
            get
            {
                var userIdValue = _httpContextAccessor.HttpContext?.User
                    .FindFirstValue(ClaimTypes.NameIdentifier);

                return int.TryParse(userIdValue, out var userId)
                    ? userId
                    : null;
            }
        }

        public string? UserName =>
            _httpContextAccessor.HttpContext?.User
                .FindFirstValue(ClaimTypes.Name);

        public string? Email =>
            _httpContextAccessor.HttpContext?.User
                .FindFirstValue(ClaimTypes.Email);

        public IReadOnlyCollection<string> Roles =>
            _httpContextAccessor.HttpContext?.User
                .FindAll(ClaimTypes.Role)
                .Select(x => x.Value)
                .ToArray()
            ?? Array.Empty<string>();

        public bool IsAuthenticated =>
            _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true;
    }
}
