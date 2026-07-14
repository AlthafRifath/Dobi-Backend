using Dobi.Contracts.Common;
using Dobi.Shared.Exceptions;
using System.Net;

namespace Dobi.Api.Middleware
{
    public sealed class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException exception)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new
                {
                    success = false,
                    message = exception.Message,
                    errors = exception.Errors
                });
            }
            catch (BadRequestException exception)
            {
                await WriteErrorAsync(context, HttpStatusCode.BadRequest, exception.Message);
            }
            catch (UnauthorizedException exception)
            {
                await WriteErrorAsync(context, HttpStatusCode.Unauthorized, exception.Message);
            }
            catch (ForbiddenException exception)
            {
                await WriteErrorAsync(context, HttpStatusCode.Forbidden, exception.Message);
            }
            catch (NotFoundException exception)
            {
                await WriteErrorAsync(context, HttpStatusCode.NotFound, exception.Message);
            }
            catch (ConflictException exception)
            {
                await WriteErrorAsync(context, HttpStatusCode.Conflict, exception.Message);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Unhandled exception occurred.");

                await WriteErrorAsync(
                    context,
                    HttpStatusCode.InternalServerError,
                    "An unexpected error occurred.");
            }
        }

        private static async Task WriteErrorAsync(
            HttpContext context,
            HttpStatusCode statusCode,
            string message)
        {
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(
                ApiResponse<object>.Fail(message));
        }
    }
}
