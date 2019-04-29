using System.Collections.Generic;
using System.Threading.Tasks;
using AssignmentsAccessor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace AssignmentsManager
{
    public static class AssignmentAccessor
    {
        [FunctionName("GetAssignments")]
        public static async Task<IActionResult> GetAssignments(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "assignments")] HttpRequest req,
            [Table("Assignments", Connection = "StorageConnection")] CloudTable assignments,
            ILogger log)
        {
            TableContinuationToken token = null;
            var entities = new List<Assignment>();

            do
            {
                var emptyQuery = new TableQuery<Assignment>();
                var queryResult = await assignments.ExecuteQuerySegmentedAsync(emptyQuery, token);
                entities.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;

            } while (token != null);

            return new OkObjectResult(entities);
        }

        [FunctionName("GetAssignment")]
        public static async Task<IActionResult> GetAssignment(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "assignments/{guid}")] HttpRequest req,
            [Table("Assignments", Connection = "StorageConnection")] CloudTable assignments,
            string guid,
            ILogger log)
        {
            return null;
        }

        [FunctionName("UpdateAssignment")]
        public static async Task<IActionResult> UpdateAssignment(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "assignments/{guid}")] HttpRequest req,
            [Table("Assignments", Connection = "StorageConnection")] CloudTable assignments,
            string guid,
            ILogger log)
        {
            return null;
        }

        [FunctionName("DeleteAssignment")]
        public static async Task<IActionResult> DeleteAssignment(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "assignments/{guid}")] HttpRequest req,
            [Table("Assignments", Connection = "StorageConnection")] CloudTable assignments,
            string guid,
            ILogger log)
        {
            return null;
        }
    }
}
