using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System;

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

        /// <inhericdoc />
        public override async Task InvokeAsync(IHttpFunctionContext context)
        {
            var header = context.Request.Headers
                .FirstOrDefault(q =>
                    q.Key.Equals("Authorization") && q.Value.Any());

            var bearerToken = header.Value == null ? string.Empty : header.Value.FirstOrDefault()?.Substring("Bearer ".Length).Trim();

            if (string.IsNullOrWhiteSpace(bearerToken))
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bearer token is required");

                return;
            }
            
            context.ClaimsPrincipal = await _tokenValidator.ValidateAsync(bearerToken);
            
            if (Next != null)
            {
                await Next.InvokeAsync(context);
            }
        }
    }
}
