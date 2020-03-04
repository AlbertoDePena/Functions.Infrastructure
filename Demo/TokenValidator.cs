using System.Security.Claims;
using System.Threading.Tasks;
using Numaka.Functions.Infrastructure;

namespace Demo
{
    public class TokenValidator : ITokenValidator
    {
        public Task<ClaimsPrincipal> ValidateAsync(string bearerToken)
        {
            var identity = new ClaimsIdentity(null, "Bearer");

            var principal = new ClaimsPrincipal(identity);

            return Task.FromResult(principal);
        }
    }
}