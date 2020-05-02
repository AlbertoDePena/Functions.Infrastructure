using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Numaka.Functions.Infrastructure
{
    /// <summary>
    /// HTTP function context boostrapper
    /// </summary>
    public interface IHttpFunctionContextBootstrapper
    {
        /// <summary>
        /// Boostrap HTTP function context
        /// </summary>
        /// <param name="request"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        IHttpFunctionContext Bootstrap(HttpRequest request, ILogger logger);
    }
}
