using Functions.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Functions.Infrastructure.Contracts;
using System;

namespace Demo
{
    public class OdometerHandler : HttpMiddleware
    {
        private readonly IGetOdometerUsingRegoQuery _getOdometerUsingRegoQuery;

        public OdometerHandler(IGetOdometerUsingRegoQuery getOdometerUsingRegoQuery)
        {
            _getOdometerUsingRegoQuery = getOdometerUsingRegoQuery ?? throw new ArgumentNullException(nameof(getOdometerUsingRegoQuery));
        }

        public override async Task InvokeAsync(IHttpFunctionContext context)
        {
            if (context.User == null)
            {
                context.Logger.LogWarning("Context does not have a user");
                
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Not good");

                return;
            }

            context.Logger.LogInformation($"Odometer request received for rego:  '{context.User.Rego}'");
            
            var odometerDto = await _getOdometerUsingRegoQuery.ExecuteAsync(context.User.Rego);

            if (odometerDto == null)
            {
                context.Response = context.Request.CreateResponse(HttpStatusCode.NotFound, "Odometor info not found");
            }
            else
            {
                context.Response = context.Request.CreateResponse(HttpStatusCode.OK, odometerDto);
            }
        }
    }
}
