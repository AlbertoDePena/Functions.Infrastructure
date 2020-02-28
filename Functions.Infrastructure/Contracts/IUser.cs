using System.Security.Claims;

namespace Functions.Infrastructure.Contracts
{
    public interface IUser
    {
        ClaimsPrincipal ClaimsPrincipal { get; }
    }
}
