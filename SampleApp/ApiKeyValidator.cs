using System.Security.Claims;
using System.Threading.Tasks;
using Numaka.Functions.Infrastructure;

namespace SampleApp
{
    public class ApiKeyValidator : IApiKeyValidator
    {
        public string HeaderName => "X-API-KEY";

        public Task<ClaimsPrincipal> ValidateAsync(string apiKey)
        {
            //TODO: validate api key

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, "API User")
            };
            
            var identity = new ClaimsIdentity(claims, "API Key");

            var principal = new ClaimsPrincipal(identity);

            return Task.FromResult(principal);
        }
    }
}