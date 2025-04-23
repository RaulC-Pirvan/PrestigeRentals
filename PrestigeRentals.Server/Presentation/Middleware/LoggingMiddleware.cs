using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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

            // Enable request body buffering to read it multiple times
            context.Request.EnableBuffering();

            string requestBody = string.Empty;

            // Read the request body
            context.Request.Body.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(context.Request.Body, leaveOpen: true))
            {
                requestBody = await reader.ReadToEndAsync();
            }
            context.Request.Body.Seek(0, SeekOrigin.Begin); // Reset so downstream can read the body

            using (var memoryStream = new MemoryStream())
            {
                context.Response.Body = memoryStream;

                // Call the next middleware in the pipeline
                await _next(context);

                stopwatch.Stop();

                // Read the response body
                memoryStream.Seek(0, SeekOrigin.Begin);
                var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();

                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    // Create a new log entry
                    var logEntry = new LogEntry
                    {
                        Path = context.Request.Path,
                        Method = context.Request.Method,
                        StatusCode = context.Response.StatusCode,
                        RequestBody = requestBody,
                        ResponseBody = responseBody,
                        UserId = context.User?.Identity?.Name ?? "Anonymous", // Ensure UserId is captured
                        DurationMs = stopwatch.ElapsedMilliseconds,
                        Timestamp = DateTime.UtcNow
                    };

                    // Save the log entry to the database
                    dbContext.Logs.Add(logEntry);
                    await dbContext.SaveChangesAsync();
                }

                // Copy the content of the memory stream to the original response stream
                memoryStream.Seek(0, SeekOrigin.Begin);
                await memoryStream.CopyToAsync(originalBodyStream);
            }
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
