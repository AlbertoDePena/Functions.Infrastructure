using System.Security.Claims;

namespace Numaka.Functions.Infrastructure.Contracts
{
    /// <summary>
    /// Generic user interface
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// The claims principal
        /// </summary>
        /// <value></value>
        ClaimsPrincipal ClaimsPrincipal { get; }
    }
}
