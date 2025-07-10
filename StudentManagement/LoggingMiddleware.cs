using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace StudentManagement
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, ILogger<LoggingMiddleware> logger)
        {
            _logger = logger;
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            _logger.LogInformation($"Request started: {httpContext.Request.Method} {httpContext.Request.Path} from {httpContext.Connection.RemoteIpAddress}");

            try
            {
                await _next(httpContext);
            }
            finally
            {
                stopwatch.Stop();

                _logger.LogInformation($"Request completed: {httpContext.Request.Method} {httpContext.Request.Path} - Status: {httpContext.Response.StatusCode} - Duration: {stopwatch.ElapsedMilliseconds}ms");
            }
        }
    }

    public static class LoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LoggingMiddleware>();
        }
    }
}