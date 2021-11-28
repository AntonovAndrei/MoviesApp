using Microsoft.AspNetCore.Builder;

namespace MoviesApp.Middleware
{
    public static class ActerLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLog(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ActerLoggingMiddleware>();
        }
    }
}