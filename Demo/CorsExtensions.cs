using System;
using System.Net;
using System.Net.Http;

namespace Demo
{
    public static class CorsExtensions
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
