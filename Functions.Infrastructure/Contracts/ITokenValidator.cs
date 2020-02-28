using System.Threading.Tasks;

namespace Functions.Infrastructure.Contracts
{
    public interface ITokenValidator
    {
        Task<IUser> ConstructPrincipal(string bearerToken);
    }
}
