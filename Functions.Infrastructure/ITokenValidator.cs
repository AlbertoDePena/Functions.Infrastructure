using System.Threading.Tasks;
using System.Security.Claims;

namespace Numaka.Functions.Infrastructure
{
    /// <summary>
    /// Token Validator interface
    /// </summary>
    public interface ITokenValidator
    {
        /// <summary>
        /// Validate a bearer token
        /// </summary>
        /// <param name="bearerToken"></param>
        Task<ClaimsPrincipal> ValidateAsync(string bearerToken);
    }
}
