using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using PrestigeRentals.Domain.Interfaces;

namespace PrestigeRentals.Presentation.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;

        public LoggingMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _scopeFactory = scopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var originalBodyStream = context.Response.Body;

            // Enable request body buffering
            context.Request.EnableBuffering();

            string requestBody;
            if (context.Request.ContentType != null &&
                context.Request.ContentType.StartsWith("multipart/form-data", StringComparison.OrdinalIgnoreCase))
            {
                requestBody = "[multipart/form-data content omitted]";
            }
            else
            {
                context.Request.Body.Seek(0, SeekOrigin.Begin);
                using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Seek(0, SeekOrigin.Begin);
            }

            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            await _next(context);

            stopwatch.Stop();

            string responseBody;
            memoryStream.Seek(0, SeekOrigin.Begin);

            // Only try to read response body if it’s text (you can customize this check)
            if (context.Response.ContentType != null &&
                context.Response.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase))
            {
                using var reader = new StreamReader(memoryStream, leaveOpen: true);
                responseBody = await reader.ReadToEndAsync();
                memoryStream.Seek(0, SeekOrigin.Begin);
            }
            else
            {
                responseBody = "[non-text response content omitted]";
            }

            using var scope = _scopeFactory.CreateScope();
            var logRepo = scope.ServiceProvider.GetRequiredService<ILogEntryRepository>();

            var logEntry = new LogEntry
            {
                Path = context.Request.Path,
                Method = context.Request.Method,
                StatusCode = context.Response.StatusCode,
                RequestBody = requestBody,
                ResponseBody = responseBody,
                UserId = context.User?.Identity?.Name ?? "Anonymous",
                DurationMs = stopwatch.ElapsedMilliseconds,
                Timestamp = DateTime.UtcNow
            };

            await logRepo.AddAsync(logEntry);

            memoryStream.Seek(0, SeekOrigin.Begin);
            await memoryStream.CopyToAsync(originalBodyStream);
        }

        // Helper method to get request body (if needed for debugging)
        private async Task<string> GetRequestBodyAsync(HttpContext context)
        {
            try
            {
                context.Request.EnableBuffering();

                context.Request.Body.Seek(0, SeekOrigin.Begin);
                using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
                var body = await reader.ReadToEndAsync();
                context.Request.Body.Seek(0, SeekOrigin.Begin);

                return string.IsNullOrWhiteSpace(body) ? "Request body was readable but empty" : body;
            }
            catch (Exception ex)
            {
                return $"Exception reading body: {ex.Message}";
            }
        }
    }
}
