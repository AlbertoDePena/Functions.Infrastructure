using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Numaka.Functions.Infrastructure
{
    /// <summary>
    /// CORS middleware
    /// </summary>
    public class CorsMiddleware : HttpMiddleware
    {
        /// <inheritdoc />
        public override async Task InvokeAsync(IHttpFunctionContext context)
        {
            if (string.Equals(context.Request.Method, "OPTIONS", StringComparison.OrdinalIgnoreCase))
            {
                var actionResult = new OkObjectResult("OK");

                if (context.Request.Headers.ContainsKey("Origin"))
                {
                    context.Request.HttpContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                    context.Request.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    context.Request.HttpContext.Response.Headers.Add("Access-Control-Allow-Methods", "GET, HEAD, OPTIONS, PUT, PATCH, POST, DELETE");
                    context.Request.HttpContext.Response.Headers.Add("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
                }

                context.ActionResult = actionResult;
            }
            else
            {
                await Next?.InvokeAsync(context);

                if (context.ActionResult != null)
                {
                    context.Request.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                }
            }
        }
    }
}
