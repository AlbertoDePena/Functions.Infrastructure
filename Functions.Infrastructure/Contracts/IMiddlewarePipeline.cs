using System.Net.Http;
using System.Threading.Tasks;
using Numaka.Functions.Infrastructure.Middlewares;

namespace Numaka.Functions.Infrastructure.Contracts
{
    public interface IMiddlewarePipeline
    {
        void Register(HttpMiddleware middleware);

        Task<HttpResponseMessage> ExecuteAsync(IHttpFunctionContext context);
    }
}
