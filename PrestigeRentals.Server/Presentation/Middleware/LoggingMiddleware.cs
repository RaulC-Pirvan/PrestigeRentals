using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Services.Interfaces;
using PrestigeRentals.Domain.Entities;
using PrestigeRentals.Infrastructure.Persistence;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;

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

            context.Request.EnableBuffering();

            string requestBody = string.Empty;
            context.Request.Body.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(context.Request.Body, leaveOpen: true))
            {
                requestBody = await reader.ReadToEndAsync();
            }
            context.Request.Body.Seek(0, SeekOrigin.Begin); // 👈 Reset so downstream can still read it

            using (var memoryStream = new MemoryStream())
            {
                context.Response.Body = memoryStream;

                await _next(context); // 👈 NOW call the next middleware

                stopwatch.Stop();

                memoryStream.Seek(0, SeekOrigin.Begin);
                var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();

                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var logEntry = new LogEntry
                {
                    Path = context.Request.Path,
                    Method = context.Request.Method,
                    StatusCode = context.Response.StatusCode,
                    RequestBody = requestBody,
                    ResponseBody = responseBody,
                    UserId = context.User?.Identity?.Name,
                    DurationMs = stopwatch.ElapsedMilliseconds,
                    Timestamp = DateTime.UtcNow
                };

                dbContext.Logs.Add(logEntry);
                await dbContext.SaveChangesAsync();

                memoryStream.Seek(0, SeekOrigin.Begin);
                await memoryStream.CopyToAsync(originalBodyStream);
            }

        }

        private async Task<string> GetRequestBodyAsync(HttpContext context)
        {
            try
            {
                Console.WriteLine("Before EnableBuffering");
                context.Request.EnableBuffering();
                Console.WriteLine("After EnableBuffering");

                context.Request.Body.Seek(0, SeekOrigin.Begin);
                using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
                var body = await reader.ReadToEndAsync();
                Console.WriteLine($"Raw body: {body}");
                context.Request.Body.Seek(0, SeekOrigin.Begin);

                if (string.IsNullOrWhiteSpace(body))
                {
                    return "Request body was readable but empty";
                }

                return body;
            }
            
            catch(Exception ex)
            {
                return $"Exception reading body: {ex.Message}";
            }
        }
    }
}
