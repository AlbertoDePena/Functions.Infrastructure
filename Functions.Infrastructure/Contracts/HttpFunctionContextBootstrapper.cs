﻿using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace Numaka.Functions.Infrastructure.Contracts
{
    /// <inhericdoc />
    public class HttpFunctionContextBootstrapper : IHttpFunctionContextBootstrapper
    {
        /// <inhericdoc />
        public IHttpFunctionContext Bootstrap(HttpRequestMessage request, ILogger logger) => new HttpFunctionContext(request, logger);
    }
}
