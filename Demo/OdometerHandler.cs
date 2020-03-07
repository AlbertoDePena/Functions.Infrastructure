using Numaka.Functions.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;

namespace Demo
{
    public class OdometerHandler : HttpMiddleware
    {
        private readonly IGetOdometerReading _getOdometerReading;

        public OdometerHandler(IGetOdometerReading getOdometerReading)
        {
            _getOdometerReading = getOdometerReading ?? throw new ArgumentNullException(nameof(getOdometerReading));
        }

        public override async Task InvokeAsync(IHttpFunctionContext context)
        {
            if (context.ClaimsPrincipal == null)
            {
                context.Logger.LogWarning("Context does not have a user");
                
                context.Response = context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Not good");

                return;
            }

            context.Logger.LogInformation($"Odometer request received for rego:  '{context.ClaimsPrincipal}'");
            
            var odometerDto = await _getOdometerReading.GetOdometerReadingAsync(context.ClaimsPrincipal.Identity.Name);

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
