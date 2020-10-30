using System.Threading.Tasks;
using System.Security.Claims;

namespace Numaka.Functions.Infrastructure
{
    /// <summary>
    /// Bearer Token Validator interface
    /// </summary>
    public interface IBearerTokenValidator
    {
        /// <summary>
        /// Validate a bearer token
        /// </summary>
        /// <param name="bearerToken"></param>
        Task<ClaimsPrincipal> ValidateAsync(string bearerToken);
    }
}
