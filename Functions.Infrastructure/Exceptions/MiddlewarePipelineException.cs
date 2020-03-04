using System;

namespace Numaka.Functions.Infrastructure.Exceptions
{
    /// <summary>
    /// Middleware pipeline exception
    /// </summary>
    public class MiddlewarePipelineException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MiddlewarePipelineException() { }

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
