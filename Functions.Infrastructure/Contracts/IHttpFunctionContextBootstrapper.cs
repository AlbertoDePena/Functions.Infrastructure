using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Functions.Infrastructure.Contracts
{
    public interface IHttpFunctionContextBootstrapper
    {
        IHttpFunctionContext Bootstrap(HttpRequestMessage request, ILogger logger);
    }
}
