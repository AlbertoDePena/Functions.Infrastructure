using System.Threading.Tasks;

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
        Task<IUser> ValidateAsync(string bearerToken);
    }
}
