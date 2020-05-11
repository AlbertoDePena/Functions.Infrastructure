using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Mvc;

namespace Numaka.Functions.Infrastructure
{
    /// <summary>
    /// Security middleware
    /// </summary>
    public class SecurityMiddleware : HttpMiddleware
    {
        private readonly ITokenValidator _tokenValidator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tokenValidator"></param>
        public SecurityMiddleware(ITokenValidator tokenValidator)
        {
            _tokenValidator = tokenValidator ?? throw new ArgumentNullException(nameof(tokenValidator));
        }

        /// <inheritdoc />
        public override async Task InvokeAsync(IHttpFunctionContext context)
        {
            var header = context.Request.Headers
                .FirstOrDefault(q =>
                    q.Key.Equals("Authorization") && q.Value.Count > 0);

            var bearerToken = header.Value.FirstOrDefault()?.Substring("Bearer ".Length).Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(bearerToken))
            {
                context.ActionResult = new BadRequestObjectResult("Bearer token is required");

                return;
            }
            
            context.ClaimsPrincipal = await _tokenValidator.ValidateTokenAsync(bearerToken);
            
            await Next?.InvokeAsync(context);
        }
    }
}
