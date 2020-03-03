﻿using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Functions.Infrastructure.Contracts
{
    public class HttpFunctionContextBootstrapper : IHttpFunctionContextBootstrapper
    {
        public IHttpFunctionContext Bootstrap(HttpRequestMessage request, ILogger logger) => new HttpFunctionContext(request, logger);
    }
}