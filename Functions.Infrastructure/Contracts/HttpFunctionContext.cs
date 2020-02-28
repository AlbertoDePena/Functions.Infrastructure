using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;

namespace Functions.Infrastructure.Contracts
{
    public class HttpFunctionContext : IHttpFunctionContext
    {
        public HttpFunctionContext(HttpRequestMessage request, ILogger logger)
        {
            Request = request ?? throw new ArgumentNullException(nameof(request));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public HttpRequestMessage Request { get; }

        public HttpResponseMessage Response { get; set; }

        public ILogger Logger { get; }

        public IUser User { get; set; }
    }
}
