using System;
using System.Threading.Tasks;
using Numaka.Functions.Infrastructure.Contracts;

namespace Numaka.Functions.Infrastructure.Middlewares
{
    public abstract class HttpMiddleware
    {
        public HttpMiddleware Next;

        protected HttpMiddleware() { }

        protected HttpMiddleware(HttpMiddleware next)
        {
            Next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public abstract Task InvokeAsync(IHttpFunctionContext context);
    }
}
