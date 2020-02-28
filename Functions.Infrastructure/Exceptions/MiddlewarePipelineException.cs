using System;

namespace Functions.Infrastructure.Exceptions
{
    public class MiddlewarePipelineException : Exception
    {
        public MiddlewarePipelineException() { }

        public MiddlewarePipelineException(string message) : base(message) { }

        public MiddlewarePipelineException(string message, Exception innerException) : base(message, innerException) { }
    }
}
