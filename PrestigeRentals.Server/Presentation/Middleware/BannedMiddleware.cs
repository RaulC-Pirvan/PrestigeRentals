namespace PrestigeRentals.Presentation.Middleware
{
    public class BannedMiddleware
    {
        private readonly RequestDelegate _next;

        public BannedMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var bannedClaim = context.User.Claims.FirstOrDefault(c => c.Type == "banned")?.Value;

            if(bool.TryParse(bannedClaim, out var isBanned) && isBanned)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Access denied: user is banned");
                return;
            }

            await _next(context);
        }
    }
}
