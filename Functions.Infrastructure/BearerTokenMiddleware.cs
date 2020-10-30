using System.Linq;
using System.Threading.Tasks;
using System;

namespace Numaka.Functions.Infrastructure
{
    /// <summary>
    /// Bearer Token middleware
    /// </summary>
    public class BearerTokenMiddleware : HttpMiddleware
    {
        private readonly IBearerTokenValidator _bearerTokenValidator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bearerTokenValidator"></param>
        public BearerTokenMiddleware(IBearerTokenValidator bearerTokenValidator)
        {
            _bearerTokenValidator = bearerTokenValidator ?? throw new ArgumentNullException(nameof(bearerTokenValidator));
        }

        /// <inheritdoc />
        public override async Task InvokeAsync(IHttpFunctionContext context)
        {
            var header = context.Request.Headers
                .FirstOrDefault(q =>
                    q.Key.Equals("Authorization") && q.Value.Count > 0);

            var bearerToken = header.Value.FirstOrDefault()?.Substring("Bearer ".Length).Trim() ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(bearerToken))
            {
                context.ClaimsPrincipal = await _bearerTokenValidator.ValidateAsync(bearerToken);
            }
            
            await Next?.InvokeAsync(context);
        }
    }
}
