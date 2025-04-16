using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Infrastructure.Persistence;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace PrestigeRentals.Presentation.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;

        // Constructor should only accept the services you need
        public LoggingMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _scopeFactory = scopeFactory;
        }

        // This method is automatically called by ASP.NET Core when the middleware is executed
        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var originalBodyStream = context.Response.Body;

            using (var memoryStream = new MemoryStream())
            {
                context.Response.Body = memoryStream;

                // Call the next middleware in the pipeline
                await _next(context);

                stopwatch.Stop();

                memoryStream.Seek(0, SeekOrigin.Begin);
                var responseBody = new StreamReader(memoryStream).ReadToEnd();

                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Log the request and response details
                var logEntry = new LogEntry
                {
                    Path = context.Request.Path,
                    Method = context.Request.Method,
                    StatusCode = context.Response.StatusCode,
                    RequestBody = await GetRequestBodyAsync(context),
                    ResponseBody = responseBody,
                    UserId = context.User?.Identity?.Name,
                    DurationMs = stopwatch.ElapsedMilliseconds,
                    Timestamp = DateTime.UtcNow
                };

                dbContext.Logs.Add(logEntry);
                await dbContext.SaveChangesAsync();

                // Rewind the memory stream to the beginning
                memoryStream.Seek(0, SeekOrigin.Begin);
                await memoryStream.CopyToAsync(originalBodyStream);
            }
        }

        private async Task<string> GetRequestBodyAsync(HttpContext context)
        {
            if (context.Request.Body.CanSeek)
            {
                context.Request.Body.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(context.Request.Body))
                {
                    return await reader.ReadToEndAsync();
                }
            }
            return string.Empty;
        }
    }
}
