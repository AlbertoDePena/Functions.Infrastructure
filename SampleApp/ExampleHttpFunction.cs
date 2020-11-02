using Functions.CustomBindings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace SampleApp
{
    public static class ExampleHttpFunction
    {
        [FunctionName("ExampleHttpFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "example")] HttpRequest request,
            ILogger logger,
            [AccessToken] AccessTokenResult accessTokenResult)
        {
            if (accessTokenResult.Status == AccessTokenStatus.Error)
            {
                logger.LogError(accessTokenResult.B2CException?.Message);
                logger.LogError(accessTokenResult.AADException?.Message);
            }
            
            var userName = accessTokenResult.Principal?.Identity.Name ?? "anonymous";

            logger.LogInformation($"Request received for {userName}.");

            return await Task.FromResult(new OkObjectResult(userName));
        }
    }
}
