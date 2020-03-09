﻿using Numaka.Functions.Infrastructure;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(SampleApp.Startup))]

namespace SampleApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IGetOdometerReading, OdometorRepository>();
            builder.Services.AddSingleton<OdometerHandler>();  
            builder.Services.AddSingleton<CorsMiddleware>();
            builder.Services.AddSingleton<SecurityMiddleware>();
            builder.Services.AddSingleton<ExceptionMiddleware>();              
            builder.Services.AddSingleton<ITokenValidator, TokenValidator>();        
            builder.Services.AddSingleton<IHttpFunctionContextBootstrapper, HttpFunctionContextBootstrapper>();
            builder.Services.AddTransient<IMiddlewarePipeline, MiddlewarePipeline>();
        }
    }
}
