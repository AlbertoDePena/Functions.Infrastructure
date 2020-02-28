using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Functions.Infrastructure.Contracts
{
    public interface IHttpFunctionContext
    {
        HttpRequestMessage Request { get; }

        HttpResponseMessage Response { get; set; }

        ILogger Logger { get; }

        IUser User { get; set; }
    }
}
