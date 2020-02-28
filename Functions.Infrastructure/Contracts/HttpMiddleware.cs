﻿using System;
using System.Threading.Tasks;

namespace Functions.Infrastructure.Contracts
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
