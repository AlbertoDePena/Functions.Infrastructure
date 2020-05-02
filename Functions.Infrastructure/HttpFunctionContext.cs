using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;

namespace Numaka.Functions.Infrastructure
{
    /// <inheritdoc/>
    public class HttpFunctionContext : IHttpFunctionContext
    {
        /// <summary>
        /// HTTP function context
        /// </summary>
        /// <param name="request"></param>
        /// <param name="logger"></param>
        public HttpFunctionContext(HttpRequest request, ILogger logger)
        {
            Request = request ?? throw new ArgumentNullException(nameof(request));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public HttpRequest Request { get; }

        /// <inheritdoc/>
        public IActionResult ActionResult { get; set; }

        /// <inheritdoc/>
        public ILogger Logger { get; }

        /// <inheritdoc/>
        public ClaimsPrincipal ClaimsPrincipal { get; set; }
    }
}
