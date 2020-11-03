using Functions.CustomBindings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace SampleApp
{
    public static class ExampleHttpFunction
    {
        private static async Task<IActionResult> ExecuteAuthenticated(ILogger logger, AccessTokenResult accessTokenResult, Func<Task<IActionResult>> actionResultFunc)
        {
            switch (accessTokenResult.Status)
            {
                case AccessTokenStatus.Error:
                    logger.LogError(accessTokenResult.B2CException?.Message);
                    logger.LogError(accessTokenResult.AADException?.Message);

                    return new InternalServerErrorResult();

                case AccessTokenStatus.Valid:
                    return await actionResultFunc();

                case AccessTokenStatus.Expired:
                case AccessTokenStatus.NoToken:
                    return new UnauthorizedResult();

                default:
                    logger.LogWarning("Access token result not supported");

                    return new UnauthorizedResult();
            }
        }

        [FunctionName("ExampleHttpFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "example")] HttpRequest request,
            ILogger logger,
            [AccessToken(b2cClientId: "%B2C_CLIENT_ID%", b2cMetadataAddress: "%B2C_METADATA_ADDRESS%", aadClientId: "%AAD_CLIENT_ID%", aadMetadataAddress: "%AAD_METADATA_ADDRESS%")] AccessTokenResult accessTokenResult)
        {
            Task<IActionResult> SayHello()
            {
                var userName = accessTokenResult.Principal?.Identity.Name ?? "anonymous";

                logger.LogInformation($"Request received for {userName}.");

                return Task.FromResult<IActionResult>(new OkObjectResult($"Hello {userName}"));
            }

            return await ExecuteAuthenticated(logger, accessTokenResult, SayHello);
        }
    }
}
