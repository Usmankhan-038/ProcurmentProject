using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace ProcurmentProject.Middleware
{
    public class ApiProtectionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string[] _blockIps = { "192.168.10.1" };
        private readonly string[] _blockAgent = { "sqlmap", "curl", "postman" };
        private readonly string[] _malaciousPayload = { "<script>", "union", "drop" };
        public ApiProtectionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var clientIp = context.Connection.RemoteIpAddress?.ToString();
            if (clientIp != null && _blockIps.Contains(clientIp))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(new { error = "Denied: This Ip Address is Blocked"
            });
                return;
            }
            var userAgent = context.Request.Headers.UserAgent.ToString().ToLower();
            if (userAgent != null && _blockAgent.Contains(userAgent))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(new { error = "Denied Request: This Operation is denied" });
                return;
            }
            var pathAndQuery = (context.Request.Path +  context.Request.Query).ToString().ToLower();
            if (_malaciousPayload.Contains(pathAndQuery))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(new { error = "Denied Request: your script is malacious" });
                return;

            }
            await _next(context);
            
        }

    }
}
