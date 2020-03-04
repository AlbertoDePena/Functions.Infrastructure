﻿using Demo;
using Numaka.Functions.Infrastructure;
using Numaka.Functions.Infrastructure.Contracts;
using Numaka.Functions.Infrastructure.Middlewares;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Demo
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IGetOdometerUsingRegoQuery, OdometorRepository>();
            builder.Services.AddSingleton<OdometerHandler>();  
            builder.Services.AddSingleton<CorsMiddleware>();
            builder.Services.AddSingleton<SecurityMiddleware>();              
            builder.Services.AddSingleton<ITokenValidator, TokenValidator>();        
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
