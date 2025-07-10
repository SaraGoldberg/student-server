using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace StudentManagement
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger<ErrorMiddleware> _logger;

        public ErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, ILogger<ErrorMiddleware> logger)
        {
            _logger = logger;
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + ex.StackTrace);
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new
            {
                error = new
                {
                    message = "An error occurred while processing your request.",
                    details = exception.Message,
                    timestamp = DateTime.UtcNow
                }
            };

            switch (exception)
            {
                case ArgumentException:
                    context.Response.StatusCode = 400;
                    break;
                case KeyNotFoundException:
                    context.Response.StatusCode = 404;
                    break;
                default:
                    context.Response.StatusCode = 500;
                    break;
            }

            var jsonResponse = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(jsonResponse);
        }
    }

    public static class ErrorMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorMiddleware>();
        }
    }
}