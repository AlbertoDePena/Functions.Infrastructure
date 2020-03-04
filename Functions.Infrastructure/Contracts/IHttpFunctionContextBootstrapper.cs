using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Numaka.Functions.Infrastructure.Contracts
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
        IHttpFunctionContext Bootstrap(HttpRequestMessage request, ILogger logger);
    }
}
