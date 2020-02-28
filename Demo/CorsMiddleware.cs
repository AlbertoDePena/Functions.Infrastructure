using Functions.Infrastructure;
using Functions.Infrastructure.Contracts;
using System;
using System.Threading.Tasks;

namespace Demo
{
    public class CorsMiddleware : HttpMiddleware
    {
        private readonly string _allowedHttpVerbs;

        public CorsMiddleware(string allowedHttpVerbs)
        {
            _allowedHttpVerbs = allowedHttpVerbs ?? throw new ArgumentNullException(nameof(allowedHttpVerbs));
        }

        public override async Task InvokeAsync(IHttpFunctionContext context)
        {
            var response = context.Request.GetCorsResponse(_allowedHttpVerbs);

            if (response == null)
            {
                await Next.InvokeAsync(context);

                if (context.Response != null)
                {
                    context.Response = context.Response.EnrichWithCorsOrigin();
                }
            }
            else
            {
                context.Response = response;
            }
        }
    }
}
