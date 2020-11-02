using System;
using System.Security.Claims;

namespace Functions.CustomBindings
{
    public sealed class AccessTokenResult
    {
        private AccessTokenResult() { }

        public ClaimsPrincipal Principal { get; private set; }

        public AccessTokenStatus Status { get; private set; }

        public Exception B2CException { get; private set; }

        public Exception AADException { get; private set; }

        internal static AccessTokenResult Success(ClaimsPrincipal principal)
            => new AccessTokenResult
            {
                Principal = principal,
                Status = AccessTokenStatus.Valid
            };

        internal static AccessTokenResult Expired()
            => new AccessTokenResult
            {
                Status = AccessTokenStatus.Expired
            };

        internal static AccessTokenResult Error(Exception b2cEx, Exception aadEx)
            => new AccessTokenResult
            {
                Status = AccessTokenStatus.Error,
                B2CException = b2cEx,
                AADException = aadEx
            };

        internal static AccessTokenResult NoToken()
            => new AccessTokenResult
            {
                Status = AccessTokenStatus.NoToken
            };
    }
}
