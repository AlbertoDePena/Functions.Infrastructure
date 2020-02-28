using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Functions.Infrastructure.Contracts
{
    public interface IHttpFunctionContextFactory
    {
        IHttpFunctionContext Bootstrap(HttpRequestMessage request, ILogger logger);
    }
}
