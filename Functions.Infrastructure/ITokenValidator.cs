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
        /// Validate a token
        /// </summary>
        /// <param name="token"></param>
        Task<ClaimsPrincipal> ValidateTokenAsync(string token);
    }
}
