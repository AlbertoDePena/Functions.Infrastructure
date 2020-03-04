using System.Security.Claims;
using System.Threading.Tasks;
using Numaka.Functions.Infrastructure;

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

    public class DemoUser : IUser
    {
        public ClaimsPrincipal ClaimsPrincipal
        {
            get
            {
                var identity = new ClaimsIdentity(null, "Bearer");

                var principal = new ClaimsPrincipal(identity);

                return principal;
            }
        }
    }
}