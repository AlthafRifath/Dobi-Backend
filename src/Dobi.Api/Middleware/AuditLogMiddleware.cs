using Dobi.Application.Abstractions.Services;
using Dobi.Shared.Constants;
using System.Text.Json;

namespace Dobi.Api.Middleware
{
    public sealed class AuditLogMiddleware
    {
        private static readonly HashSet<string> WriteMethods = new(StringComparer.OrdinalIgnoreCase)
        {
            "POST",
            "PUT",
            "PATCH",
            "DELETE"
        };

        private readonly RequestDelegate _next;
        private readonly ILogger<AuditLogMiddleware> _logger;

        public AuditLogMiddleware(
            RequestDelegate next,
            ILogger<AuditLogMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var shouldAudit = ShouldAudit(context);

            await _next(context);

            if (!shouldAudit)
            {
                return;
            }

            try
            {
                var auditLogService = context.RequestServices
                    .GetRequiredService<IAuditLogService>();

                var userId = TryGetUserId(context);
                var entityName = GetEntityNameFromPath(context.Request.Path.Value);
                var entityId = TryGetEntityIdFromPath(context.Request.Path.Value);

                var auditPayload = JsonSerializer.Serialize(new
                {
                    method = context.Request.Method,
                    path = context.Request.Path.Value,
                    queryString = context.Request.QueryString.Value,
                    statusCode = context.Response.StatusCode
                });

                await auditLogService.LogAsync(
                    new CreateAuditLogRequest(
                        entityName,
                        entityId,
                        AuditActionCodes.ApiWriteAction,
                        null,
                        auditPayload,
                        userId,
                        GetIpAddress(context),
                        context.Request.Headers.UserAgent.ToString()),
                    context.RequestAborted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to write audit log.");
            }
        }

        private static bool ShouldAudit(HttpContext context)
        {
            if (!WriteMethods.Contains(context.Request.Method))
            {
                return false;
            }

            var path = context.Request.Path.Value ?? string.Empty;

            if (!path.StartsWith("/api", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (path.StartsWith("/api/auth/login", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (path.StartsWith("/api/auth/refresh-token", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }

        private static int? TryGetUserId(HttpContext context)
        {
            var userIdValue = context.User.FindFirst("userId")?.Value
                              ?? context.User.FindFirst("sub")?.Value
                              ?? context.User.FindFirst("nameid")?.Value
                              ?? context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            return int.TryParse(userIdValue, out var userId)
                ? userId
                : null;
        }

        private static string GetEntityNameFromPath(string? path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return "Unknown";
            }

            var segments = path
                .Split('/', StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            if (segments.Length < 2)
            {
                return "Unknown";
            }

            return segments[1];
        }

        private static int? TryGetEntityIdFromPath(string? path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return null;
            }

            var segments = path
                .Split('/', StringSplitOptions.RemoveEmptyEntries);

            foreach (var segment in segments)
            {
                if (int.TryParse(segment, out var id))
                {
                    return id;
                }
            }

            return null;
        }

        private static string? GetIpAddress(HttpContext context)
        {
            return context.Connection.RemoteIpAddress?.ToString();
        }
    }
}
