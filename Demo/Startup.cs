using Demo;
using Functions.Infrastructure;
using Functions.Infrastructure.Contracts;
using Functions.Infrastructure.Middlewares;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Demo
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<ITokenValidator, TokenValidator>();
            builder.Services.AddSingleton<IUser, DemoUser>();
            builder.Services.AddSingleton<IGetOdometerUsingRegoQuery, OdometorRepository>();
            builder.Services.AddSingleton(new CorsMiddleware(allowedHttpVerbs: "GET, OPTIONS"));
            builder.Services.AddSingleton<SecurityMiddleware>();
            builder.Services.AddSingleton<OdometerHandler>();            
            builder.Services.AddSingleton<IHttpFunctionContextBootstrapper, HttpFunctionContextBootstrapper>();
            builder.Services.AddSingleton<IMiddlewarePipeline>(provider =>
            {
                var pipeline = new MiddlewarePipeline();

                // Order of middleware matters!!!
                pipeline.Register(provider.GetService<CorsMiddleware>());
                pipeline.Register(provider.GetService<SecurityMiddleware>());
                pipeline.Register(provider.GetService<OdometerHandler>());

                return pipeline;
            });
        }
    }
}
