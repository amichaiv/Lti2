using System;
using System.IO;
using System.Threading.Tasks;
using AzureAccessor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureResourcesManager
{
    public static class ResourcesCrud
    {
        [FunctionName("GetResources")]
        public static async Task<IActionResult> GetResources(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "resources")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var resourcesDal = new ResourcesDal();
            var resources = resourcesDal.GetResources();
            return new OkObjectResult(resources);
        }
    }
}
