using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AssignmentsAccessor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using StudentsAccessor;

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
            //var membershipsManager = new LmsMemberships();
            var assignmentsEntities = await GetAssignmentsAsync(assignments, new TableQuery<Assignment>());
            //foreach (var assignment in assignmentsEntities)
            //{
            //    var members = await membershipsManager.GetMemberships(assignment.CustomContextMembershipsUrl,
            //        assignment.OAuthConsumerKey, "secret", assignment.ResourceLinkId);
            //    assignment.Members = MapMembersToPersons(members);
            //}
            return new OkObjectResult(assignmentsEntities);
        }

        [FunctionName("GetAssignment")]
        public static async Task<IActionResult> GetAssignment(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "assignments/{guid}/users/{userId}")] HttpRequest req,
            [Table("Assignments", Connection = "StorageConnection")] CloudTable assignments,
            string guid,
            string userId,
            ILogger log)
        {
            var query = new TableQuery<Assignment>();
            var assignment = (await GetAssignmentsAsync(assignments, query))?.FirstOrDefault();
            if (assignment == null)
            {
                return new NotFoundObjectResult(guid);
            }

            assignment.Name = "SQL";
            assignment.Members = Person.GetPersons().ToList();
            assignment.NoOfStudents = assignment.Members.Count();
            assignment.TotalConsumed = assignment.Members.Sum(member => member.Consumed);
            assignment.NoOfProjectGroups = assignment.Members.Select(member => member.Group).Distinct().Count();
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

        public static async Task<List<Assignment>> GetAssignmentsAsync(CloudTable assignments, TableQuery<Assignment> query)
        {
            TableContinuationToken token = null;
            var entities = new List<Assignment>();

            do
            {
                var queryResult = await assignments.ExecuteQuerySegmentedAsync(query, token);
                entities.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;

            } while (token != null);

            return entities;
        }


    }
}
