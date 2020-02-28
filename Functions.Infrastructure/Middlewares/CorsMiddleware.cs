using Functions.Infrastructure.Contracts;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Functions.Infrastructure.Middlewares
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

    internal static class CorsExtensions
    {
        public static HttpResponseMessage GetCorsResponse(this HttpRequestMessage request, string allowedHttpVerbs)
        {
            if (string.Equals(request.Method.Method, "OPTIONS", StringComparison.OrdinalIgnoreCase))
            {
                var response = request.CreateResponse(HttpStatusCode.OK, "Hello from the other side");

                if (request.Headers.Contains("Origin"))
                {
                    response.Headers.Add("Access-Control-Allow-Credentials", "true");
                    response.Headers.Add("Access-Control-Allow-Origin", "*");
                    response.Headers.Add("Access-Control-Allow-Methods", allowedHttpVerbs.ToUpper());
                    response.Headers.Add("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
                }

                return response;
            }

            return null;
        }

        public static HttpResponseMessage EnrichWithCorsOrigin(this HttpResponseMessage response)
        {
            response.Headers.Add("Access-Control-Allow-Origin", "*");

            return response;
        }
    }
}
