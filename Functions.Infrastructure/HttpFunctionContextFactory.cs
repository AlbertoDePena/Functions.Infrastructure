using Functions.Infrastructure.Contracts;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Functions.Infrastructure
{
    public class HttpFunctionContextFactory : IHttpFunctionContextFactory
    {
        public IHttpFunctionContext Bootstrap(HttpRequestMessage request, ILogger logger) => new HttpFunctionContext(request, logger);
    }
}
