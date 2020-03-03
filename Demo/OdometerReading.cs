﻿using Functions.Infrastructure.Contracts;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Demo
{
    public class OdometerReading
    {
        private readonly IHttpFunctionContextBootstrapper _bootstrapper;
        private readonly IMiddlewarePipeline _pipeline;

        public OdometerReading(IHttpFunctionContextBootstrapper bootstrapper, IMiddlewarePipeline pipeline)
        {
            _bootstrapper = bootstrapper ?? throw new ArgumentNullException(nameof(bootstrapper));
            _pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
        }

        [FunctionName("OdometerReading")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", "OPTIONS")]HttpRequestMessage request, ILogger logger)
        {
            logger.LogInformation("Bootstrapping HTTP function context...");

            var context = _bootstrapper.Bootstrap(request, logger);

            logger.LogInformation("Executing request...");

            return await _pipeline.ExecuteAsync(context);
        }
    }
}
