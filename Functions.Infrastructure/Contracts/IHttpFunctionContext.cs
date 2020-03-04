using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Numaka.Functions.Infrastructure.Contracts
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
        /// Generic user interface
        /// </summary>
        IUser User { get; set; }
    }
}
