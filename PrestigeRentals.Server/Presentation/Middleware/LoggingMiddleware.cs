using PrestigeRentals.Application.DTO;
using PrestigeRentals.Application.Services.Interfaces;
using System.Diagnostics;

namespace PrestigeRentals.Presentation.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        
        public LoggingMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
;       }

        public async Task Invoke(HttpContext context, ILogService logger)
        {
            var stopwatch = Stopwatch.StartNew();
            await _requestDelegate(context);
            stopwatch.Stop();

            var logEntry = new LogEntryDTO
            {
                Path = context.Request.Path,
                Method = context.Request.Method,
                StatusCode = context.Response.StatusCode,
                DurationMs = stopwatch.ElapsedMilliseconds,
                Timestamp = DateTime.UtcNow
            };

            await logger.LogAsync(logEntry);
        }
    }
}
