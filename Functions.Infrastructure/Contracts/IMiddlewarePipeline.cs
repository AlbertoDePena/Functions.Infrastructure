using System.Net.Http;
using System.Threading.Tasks;
using Functions.Infrastructure.Middlewares;

namespace Functions.Infrastructure.Contracts
{
    public interface IMiddlewarePipeline
    {
        void Register(HttpMiddleware middleware);

        Task<HttpResponseMessage> ExecuteAsync(IHttpFunctionContext context);
    }
}
