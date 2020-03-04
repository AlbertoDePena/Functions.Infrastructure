using System.Security.Claims;

namespace Numaka.Functions.Infrastructure.Contracts
{
    public interface IUser
    {
        ClaimsPrincipal ClaimsPrincipal { get; }
    }
}
