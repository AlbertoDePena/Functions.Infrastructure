using Functions.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Functions.Infrastructure.Contracts;

namespace Demo
{
    public class SecurityMiddleware : HttpMiddleware
    {
        private readonly bool _mustBeAuthenticated;
        private readonly ITokenValidator _tokenValidator;

        public SecurityMiddleware(bool mustBeAuthenticated, ITokenValidator tokenValidator)
        {
            _mustBeAuthenticated = mustBeAuthenticated;
            _tokenValidator = tokenValidator;
        }

        public override async Task InvokeAsync(IHttpFunctionContext context)
        {
            var header = context.Request.Headers
                .FirstOrDefault(q =>
                    q.Key.Equals("Authorization") && q.Value.Any());

            var bearerToken = header.Value == null ? string.Empty : header.Value.FirstOrDefault()?.Substring("Bearer ".Length).Trim();

            if (string.IsNullOrWhiteSpace(bearerToken))
            {
                context.Logger.LogWarning("No bearer token provided");

                if (_mustBeAuthenticated)
                {
                    context.Response = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bearer token can't be empty");

                    return;
                }
            }
            else
            {
                context.User = await _tokenValidator.ConstructPrincipal(bearerToken);
            }

            if (Next != null)
            {
                await Next.InvokeAsync(context);
            }
        }
    }
}
