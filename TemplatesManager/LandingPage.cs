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

            LtiRequest ltiRequest = await req.ParseLtiRequestAsync();
            var assignmentToDb = new Assignment(ltiRequest);
            Assignment assignment = await GetOrAddToDbAsync(assignments, assignmentToDb);

            var ulrWithParams = $"{RedirectUrl}?assignmentGuid={assignment.Guid}&userId={ltiRequest.UserId}";
            return new RedirectToPageResult(ulrWithParams);
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
