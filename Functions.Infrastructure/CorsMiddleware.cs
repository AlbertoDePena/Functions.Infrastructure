using Microsoft.AspNetCore.Http;
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
            var actionResult = context.Request.GetCorsActionResult();

            if (actionResult == null)
            {
                await Next?.InvokeAsync(context);

                if (context.ActionResult != null)
                {
                    context.Request.EnrichWithCorsOrigin();
                }
            }
            else
            {
                context.ActionResult = actionResult;
            }
        }
    }

    internal static class CorsExtensions
    {
        public static IActionResult GetCorsActionResult(this HttpRequest request)
        {
            if (string.Equals(request.Method, "OPTIONS", StringComparison.OrdinalIgnoreCase))
            {
                var actionResult = new OkObjectResult("Hello from the other side");

                if (request.Headers.ContainsKey("Origin"))
                {
                    request.HttpContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                    request.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    request.HttpContext.Response.Headers.Add("Access-Control-Allow-Methods", "GET, HEAD, OPTIONS, PUT, PATCH, POST, DELETE");
                    request.HttpContext.Response.Headers.Add("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
                }

                return actionResult;
            }

            return null;
        }

        public static void EnrichWithCorsOrigin(this HttpRequest request) => request.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
    }
}
