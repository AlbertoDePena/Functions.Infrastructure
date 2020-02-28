using System.Threading.Tasks;
using Functions.Infrastructure.Contracts;

namespace Demo
{
    public class TokenValidator : ITokenValidator
    {
        public Task<IUser> ValidateAsync(string bearerToken)
        {
            var user = new DemoUser();

            return Task.FromResult(user as IUser);
        }
    }
}