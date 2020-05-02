using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Numaka.Functions.Infrastructure
{
    /// <inheritdoc />
    public class HttpFunctionContextBootstrapper : IHttpFunctionContextBootstrapper
    {
        /// <inheritdoc />
        public IHttpFunctionContext Bootstrap(HttpRequest request, ILogger logger) => new HttpFunctionContext(request, logger);
    }
}
