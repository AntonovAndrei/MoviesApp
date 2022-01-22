using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class ActerLoggingMiddleware
{
    private readonly RequestDelegate _next;
    
    public ActerLoggingMiddleware(RequestDelegate next)
    {
        this._next = next;
    }
    
    public async Task InvokeAsync(HttpContext context, ILogger<ActerLoggingMiddleware> logger)
    {
        if (context.Request.Path.ToString().Contains("/Acters"))
        {
            StringBuilder strContextValue = new StringBuilder();
            if (context.Request.Method == "POST")
            {
                var contextForm = context.Request.Form;

                foreach (var cf in contextForm)
                {
                    strContextValue.AppendLine(cf.Key + "- " + cf.Value);
                }
                
                logger.LogInformation($"Request path: {context.Request.Path}\n Method: {context.Request.Method}\n Params:\n {strContextValue}");
            }
            else
            {
                logger.LogInformation($"Request path: {context.Request.Path}\n Method: {context.Request.Method}");
            }
        }

        await _next(context);
    }
}