using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        HttpRequest Request { get; }

        /// <summary>
        /// The action result
        /// </summary>
        IActionResult ActionResult { get; set; }

        /// <summary>
        /// Microsoft logger
        /// </summary>
        ILogger Logger { get; }

        /// <summary>
        /// The claims principal. The HTTP Context has the function's claims principal which is not the same as this one.
        /// </summary>
        ClaimsPrincipal ClaimsPrincipal { get; set; }
    }
}
