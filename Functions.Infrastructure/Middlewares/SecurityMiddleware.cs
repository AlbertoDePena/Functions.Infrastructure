using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Functions.Infrastructure.Contracts;
using System;

namespace Functions.Infrastructure.Middlewares
{
    public class SecurityMiddleware : HttpMiddleware
    {
        private readonly ITokenValidator _tokenValidator;

        public SecurityMiddleware(ITokenValidator tokenValidator)
        {
            _tokenValidator = tokenValidator ?? throw new ArgumentNullException(nameof(tokenValidator));
        }

        public override async Task InvokeAsync(IHttpFunctionContext context)
        {
            var header = context.Request.Headers
                .FirstOrDefault(q =>
                    q.Key.Equals("Authorization") && q.Value.Any());

            var bearerToken = header.Value == null ? string.Empty : header.Value.FirstOrDefault()?.Substring("Bearer ".Length).Trim();

            if (string.IsNullOrWhiteSpace(bearerToken))
            {
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bearer token can't be empty");

                return;
            }
            else
            {
                context.User = await _tokenValidator.ValidateAsync(bearerToken);
            }

            if (Next != null)
            {
                await Next.InvokeAsync(context);
            }
        }
    }
}
