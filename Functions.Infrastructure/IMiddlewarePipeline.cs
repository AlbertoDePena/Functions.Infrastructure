using System.Net.Http;
using System.Threading.Tasks;

namespace Numaka.Functions.Infrastructure
{
    /// <summary>
    /// Middleware pipeline interface
    /// </summary>
    public interface IMiddlewarePipeline
    {
        /// <summary>
        /// Register a HTTP middleware
        /// </summary>
        void Register(HttpMiddleware middleware);

        /// <summary>
        /// Execute a HTTP request
        /// </summary>
        /// <param name="context">The HTTP function context</param>
        /// <returns></returns>
        Task<HttpResponseMessage> ExecuteAsync(IHttpFunctionContext context);
    }
}
