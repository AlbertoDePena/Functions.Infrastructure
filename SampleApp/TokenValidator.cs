using System.Security.Claims;
using System.Threading.Tasks;
using Numaka.Functions.Infrastructure;

namespace SampleApp
{
    public class TokenValidator : ITokenValidator
    {
        public Task<ClaimsPrincipal> ValidateAsync(string bearerToken)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, "Test User")
            };
            
            var identity = new ClaimsIdentity(claims, "Bearer");

            var principal = new ClaimsPrincipal(identity);

            return Task.FromResult(principal);
        }
    }
}