using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Security.Claims;

namespace Numaka.Functions.Infrastructure
{
    /// <summary>
    /// HTTP function context interface
    /// </summary>
    public interface IHttpFunctionContext
    {
        /// <summary>
        /// The HTTP request
        /// </summary>
        HttpRequestMessage Request { get; }

        /// <summary>
        /// The HTTP response
        /// </summary>
        HttpResponseMessage Response { get; set; }

        /// <summary>
        /// Microsoft logger
        /// </summary>
        ILogger Logger { get; }

        /// <summary>
        /// The claims principal
        /// </summary>
        ClaimsPrincipal ClaimsPrincipal { get; set; }
    }
}
