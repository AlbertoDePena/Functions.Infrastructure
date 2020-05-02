using Numaka.Functions.Infrastructure;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Mvc;

namespace SampleApp
{
    public class ExceptionMiddleware : HttpMiddleware
    {
        public override async Task InvokeAsync(IHttpFunctionContext context)
        {
            try
            {
                if (Next != null)
                {
                    await Next.InvokeAsync(context);
                }
            }
            catch (Exception e)
            {
                // Map each exception to appropriate HTTP status code/message...
                
                var message = $"ExceptionMiddleware: {e.Message}";

                context.Logger.LogError(e, message);

                context.ActionResult = new BadRequestObjectResult(message);
            }
        }
    }
}