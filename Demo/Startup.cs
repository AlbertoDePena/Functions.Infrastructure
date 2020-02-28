﻿using Demo;
using Functions.Infrastructure;
using Functions.Infrastructure.Contracts;
using Functions.Infrastructure.Middlewares;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

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
            builder.Services.AddSingleton(provider => new SecurityMiddleware(provider.GetService<ITokenValidator>(), mustBeAuthenticated: true));
            builder.Services.AddSingleton<OdometerHandler>();            
            builder.Services.AddSingleton<IHttpFunctionContextFactory, HttpFunctionContextFactory>();
            builder.Services.AddSingleton<IMiddlewarePipeline>(provider =>
            {
                // Order of middleware matters!!!
                return new MiddlewarePipeline(new List<HttpMiddleware>()
                {
                    provider.GetService<CorsMiddleware>(),
                    provider.GetService<SecurityMiddleware>(),
                    provider.GetService<OdometerHandler>()
                });
            });
        }
    }
}
