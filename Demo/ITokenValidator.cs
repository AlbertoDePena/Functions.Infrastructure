using Functions.Infrastructure.Contracts;
using System.Threading.Tasks;

namespace Demo
{
    public interface ITokenValidator
    {
        Task<IUser> ConstructPrincipal(string bearerToken);
    }
}
