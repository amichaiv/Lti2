using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace AssignmentsManager
{
    public static class AssignmentsApi
    {
        [FunctionName("GetAssignments")]
        public static async Task<IActionResult> GetAssignments(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "assignments")] HttpRequest req,
            [Table("Assignments", Connection = "StorageConnection")] CloudTable assignments,
            ILogger log)
        {
            var entities = new List<Assignment>();
            var emptyQuery = new TableQuery<Assignment>();
            TableContinuationToken token = null;

            do
            {
                var queryResult = await assignments.ExecuteQuerySegmentedAsync(emptyQuery, token);
                if (queryResult.Results.Any())
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

            var filter = TableQuery
                .GenerateFilterConditionForGuid("Guid", QueryComparisons.Equal, Guid.Parse(guid));
      
            var query = new TableQuery<Assignment>().Where(filter).Take(1);
            var queryResult = await assignments.ExecuteQuerySegmentedAsync(query, null);
            var assignment = queryResult.Results.FirstOrDefault();
            if (assignment == null)
            {
                return new NotFoundObjectResult(guid);
            }

            return new OkObjectResult(assignment);
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

        //public static async Task<List<Assignment>> GetAssignmentsAsync(CloudTable assignments, TableQuery<Assignment> query)
        //{
        //    TableContinuationToken token = null;
        //    var entities = new List<Assignment>();

        //    do
        //    {
        //        var queryResult = await assignments.ExecuteQuerySegmentedAsync(query, token);
        //        if (queryResult.Results.Any())
        //            entities.AddRange(queryResult.Results);
        //        token = queryResult.ContinuationToken;

        //    } while (token != null);

        //    return entities;
        //}


    }
}
