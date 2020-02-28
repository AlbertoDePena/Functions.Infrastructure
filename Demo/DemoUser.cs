using Functions.Infrastructure.Contracts;
using System.Security.Claims;

namespace Demo
{
    public class DemoUser : IUser
    {
        public ClaimsPrincipal ClaimsPrincipal => throw new System.NotImplementedException();
    }
}