using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MoviesApp.Middleware
{
    public class RequestLogMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, ILogger<RequestLogMiddleware> logger)
        {
            logger.LogTrace($"Request: {httpContext.Request.Path}  Method: {httpContext.Request.Method}");
            await _next(httpContext);
        }
    }
}