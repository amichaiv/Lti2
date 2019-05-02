using System;
using System.Threading.Tasks;
using AssignmentsAccessor;
using LtiLibrary.AspNetCore.Extensions;
using LtiLibrary.NetCore.Lti.v1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace TemplatesManager
{
    public static class LandingPage
    {
        private static readonly string RedirectUrl = Environment.GetEnvironmentVariable("RedirectUrl");

        [FunctionName("connect")]
        public static async Task<IActionResult> Connect(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            [Table("Assignments", Connection = "StorageConnection")] CloudTable assignments,
            ILogger log)
        {
            log.LogInformation("New LMS Connection");

            var assignment = await ParseToAssignment(req);
            Assignment assignment1 = await GetOrAddToDbAsync(assignments, assignment);

            return new RedirectToPageResult(RedirectUrl);
        }

        private static async Task<Assignment> ParseToAssignment(HttpRequest req)
        {
            LtiRequest ltiRequest = await req.ParseLtiRequestAsync();
            Assignment assignment = new Assignment(ltiRequest);
            
            return assignment;
        }

        private static async Task<Assignment> GetOrAddToDbAsync(CloudTable assignments, Assignment assignment)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<Assignment>(assignment.PartitionKey, assignment.RowKey);
            TableResult tableResult = await assignments.ExecuteAsync(retrieveOperation);

            if (tableResult.Result is Assignment result)
            {
                return result;
            }

            assignment.GenerateGuid();
            
            TableOperation tableOperation = TableOperation.InsertOrMerge(assignment);
            await assignments.ExecuteAsync(tableOperation);
            return assignment;
        }
    }
}
