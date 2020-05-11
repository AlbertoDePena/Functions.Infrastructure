using System;

namespace Numaka.Functions.Infrastructure
{
    /// <summary>
    /// Middleware pipeline exception
    /// </summary>
    public class MiddlewarePipelineException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MiddlewarePipelineException()
            : this("Either the middleware pipeline did not have any middlewares registered or the middleware invocation did not yield an action result") { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        public MiddlewarePipelineException(string message) : base(message) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public MiddlewarePipelineException(string message, Exception innerException) : base(message, innerException) { }
    }
}
