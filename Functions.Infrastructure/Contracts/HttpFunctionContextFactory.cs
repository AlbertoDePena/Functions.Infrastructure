using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Functions.Infrastructure.Contracts
{
    public class HttpFunctionContextFactory : IHttpFunctionContextFactory
    {
        public IHttpFunctionContext Bootstrap(HttpRequestMessage request, ILogger logger) => new HttpFunctionContext(request, logger);
    }
}
