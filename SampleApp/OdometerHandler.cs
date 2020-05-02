using Numaka.Functions.Infrastructure;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace SampleApp
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
                context.ActionResult = new BadRequestObjectResult("Context does not have a claims principal");

                return;
            }

            context.Request.Query.TryGetValue("vin", out StringValues vin);

            if (string.IsNullOrWhiteSpace(vin))
            {
                context.ActionResult = new BadRequestObjectResult("vin is required");

                return;
            }

            context.Logger.LogInformation("Odometer request received for: {UserName}", context.ClaimsPrincipal.Identity.Name);
            
            var odometerDto = await _getOdometerReading.GetOdometerReadingAsync(vin);

            if (odometerDto == null)
            {
                context.ActionResult = new NotFoundObjectResult("Odometor info not found");
            }
            else
            {
                context.ActionResult = new OkObjectResult(odometerDto);
            }
        }
    }
}
