using System.Net;

namespace ApiGateway.Middlewares
{
    public class IPWhitelistMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<IPWhitelistMiddleware> _logger;
        private readonly HashSet<string> _whitelistedIPs;

        public IPWhitelistMiddleware(RequestDelegate next, ILogger<IPWhitelistMiddleware> logger, IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _whitelistedIPs = configuration.GetSection("WhitelistedIPs").Get<HashSet<string>>() ?? new HashSet<string>();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //Заглушка!!! Потом убрать!!!
            //var remoteIp = context.Connection.RemoteIpAddress?.ToString();
            //if (remoteIp != null && !_whitelistedIPs.Contains(remoteIp))
            //{
            //    _logger.LogWarning($"Forbidden request from IP: {remoteIp}");
            //    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            //    await context.Response.WriteAsync("Forbidden");
            //    return;
            //}

            await _next(context);
        }
    }
}
