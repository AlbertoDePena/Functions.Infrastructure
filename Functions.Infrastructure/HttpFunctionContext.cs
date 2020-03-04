using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Security.Claims;

namespace Numaka.Functions.Infrastructure
{
    /// <inhericdoc />
    public class HttpFunctionContext : IHttpFunctionContext
    {
        /// <summary>
        /// HTTP function context
        /// </summary>
        /// <param name="request"></param>
        /// <param name="logger"></param>
        public HttpFunctionContext(HttpRequestMessage request, ILogger logger)
        {
            Request = request ?? throw new ArgumentNullException(nameof(request));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inhericdoc />
        public HttpRequestMessage Request { get; }

        /// <inhericdoc />
        public HttpResponseMessage Response { get; set; }

        /// <inhericdoc />
        public ILogger Logger { get; }

        /// <inhericdoc />
        public ClaimsPrincipal ClaimsPrincipal { get; set; }
    }
}
