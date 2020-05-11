﻿using System.Threading.Tasks;

namespace Numaka.Functions.Infrastructure
{
    /// <summary>
    /// HTTP middleware
    /// </summary>
    public abstract class HttpMiddleware
    {
        /// <summary>
        /// The next middleware
        /// </summary>
        public HttpMiddleware Next;

        /// <summary>
        /// Constructor
        /// </summary>
        protected HttpMiddleware() { }

        /// <summary>
        /// Invoke a HTTP request
        /// </summary>
        /// <param name="context">The HTTP function context</param>
        public abstract Task InvokeAsync(IHttpFunctionContext context);
    }
}
