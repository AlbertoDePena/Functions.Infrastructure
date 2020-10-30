using Numaka.Functions.Infrastructure;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SampleApp
{
    public class OdometerReading
    {
        private readonly IServiceProvider _serviceProvider;

        public OdometerReading(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        [FunctionName("OdometerReadingWithBearerToken")]
        public async Task<IActionResult> OdometerReadingWithBearerToken(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options")] HttpRequest request, ILogger logger)
        {
            var bootstrapper = _serviceProvider.GetService<IHttpFunctionContextBootstrapper>();
            var pipeline = _serviceProvider.GetService<IMiddlewarePipeline>();

            logger.LogInformation("Bootstrapping HTTP function context...");

            var context = bootstrapper.Bootstrap(request, logger);

            logger.LogInformation("Registering middlewares...");

            // Order of middleware matters!!!
            pipeline.Register(_serviceProvider.GetService<CorsMiddleware>());
            pipeline.Register(_serviceProvider.GetService<ExceptionMiddleware>());
            pipeline.Register(_serviceProvider.GetService<BearerTokenMiddleware>());            
            pipeline.Register(_serviceProvider.GetService<OdometerHandler>());

            logger.LogInformation("Executing request...");

            return await pipeline.ExecuteAsync(context);
        }

        [FunctionName("OdometerReadingWithApiKey")]
        public async Task<IActionResult> OdometerReadingWithApiKey(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options")] HttpRequest request, ILogger logger)
        {
            var bootstrapper = _serviceProvider.GetService<IHttpFunctionContextBootstrapper>();
            var pipeline = _serviceProvider.GetService<IMiddlewarePipeline>();

            logger.LogInformation("Bootstrapping HTTP function context...");

            var context = bootstrapper.Bootstrap(request, logger);

            logger.LogInformation("Registering middlewares...");

            // Order of middleware matters!!!
            pipeline.Register(_serviceProvider.GetService<CorsMiddleware>());
            pipeline.Register(_serviceProvider.GetService<ExceptionMiddleware>());
            pipeline.Register(_serviceProvider.GetService<ApiKeyMiddleware>());            
            pipeline.Register(_serviceProvider.GetService<OdometerHandler>());

            logger.LogInformation("Executing request...");

            return await pipeline.ExecuteAsync(context);
        }
    }
}
