using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host.Bindings;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Functions.CustomBindings
{
    public class AccessTokenValueProvider : IValueProvider
    {
        private readonly Func<HttpRequest, Task<AccessTokenResult>> _accessTokenResultFactory;
        private readonly HttpRequest _request;
        
        public AccessTokenValueProvider(Func<HttpRequest, Task<AccessTokenResult>> accessTokenResultFactory, HttpRequest request)
        {
            _accessTokenResultFactory = accessTokenResultFactory ?? throw new ArgumentNullException(nameof(accessTokenResultFactory));
            _request = request ?? throw new ArgumentNullException(nameof(request));
        }

        public async Task<object> GetValueAsync() => await _accessTokenResultFactory(_request);

        public Type Type => typeof(ClaimsPrincipal);

        public string ToInvokeString() => string.Empty;
    }
}
