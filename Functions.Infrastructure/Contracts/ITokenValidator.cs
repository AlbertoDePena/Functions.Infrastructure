using System.Threading.Tasks;

namespace Numaka.Functions.Infrastructure.Contracts
{
    public interface ITokenValidator
    {
        Task<IUser> ValidateAsync(string bearerToken);
    }
}
