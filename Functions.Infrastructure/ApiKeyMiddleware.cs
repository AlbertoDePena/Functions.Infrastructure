using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Primitives;

namespace Numaka.Functions.Infrastructure
{
    /// <summary>
    /// API key middleware
    /// </summary>
    public class ApiKeyMiddleware : HttpMiddleware
    {
        private readonly IApiKeyValidator _apiKeyValidator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="apiKeyValidator"></param>
        public ApiKeyMiddleware(IApiKeyValidator apiKeyValidator)
        {
            _apiKeyValidator = apiKeyValidator ?? throw new ArgumentNullException(nameof(apiKeyValidator));
        }

        /// <inheritdoc />
        public override async Task InvokeAsync(IHttpFunctionContext context)
        {
            var headerKey = _apiKeyValidator.HeaderName;

            var header = context.Request.Headers.TryGetValue(headerKey, out StringValues apiKey);

            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                context.ClaimsPrincipal = await _apiKeyValidator.ValidateAsync(apiKey);
            }

            await Next?.InvokeAsync(context);
        }
    }
}