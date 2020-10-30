using System.Threading.Tasks;
using System.Security.Claims;

namespace Numaka.Functions.Infrastructure
{
     /// <summary>
    /// API key Validator interface
    /// </summary>
    public interface IApiKeyValidator
    {
        /// <summary>
        /// Validate an API key
        /// </summary>
        /// <param name="apiKey"></param>
        Task<ClaimsPrincipal> ValidateAsync(string apiKey);

        /// <summary>
        /// Get the API key header name
        /// </summary>
        string HeaderName { get; }
    }
}