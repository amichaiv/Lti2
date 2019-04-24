using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Threading.Tasks;
using LtiLibrary.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace TemplatesManager
{
    public static class LandingPage
    {
        private static readonly string RedirectUrl = Environment.GetEnvironmentVariable("RedirectUrl");

        [FunctionName("connect")]
        public static async Task<IActionResult> Connect(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            [Microsoft.Azure.WebJobs.Table("Assignments", Connection = "StorageConnection")] CloudTable assignments,
            ILogger log)
        {
            log.LogInformation("New LMS Connection");

            var assigment = await ParseToAssigment(req);
            Assigment assigment1 = await GetOrAddToDbAsync(assignments, assigment);

            return new RedirectToPageResult(RedirectUrl);
        }

        private static async Task<Assigment> ParseToAssigment(HttpRequest req)
        {
            var ltiRequest = await req.ParseLtiRequestAsync();

            Assigment assigment = new Assigment(ltiRequest.ToolConsumerInstanceName,
                ltiRequest.ContextId,
                ltiRequest.ResourceLinkId);
            return assigment;
        }

        private static async Task<Assigment> GetOrAddToDbAsync(CloudTable assignments, Assigment assigment)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<Assigment>(assigment.PartitionKey, assigment.RowKey);
            TableResult tableResult = await assignments.ExecuteAsync(retrieveOperation);

            if (tableResult.Result is Assigment result)
            {
                return result;
            }

            assigment.GenerateGuid();
            TableOperation tableOperation = TableOperation.Insert(assigment);
            await assignments.ExecuteAsync(tableOperation);
            return assigment;
        }
    }
}
