using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AssignmentsAccessor;
using LtiLibrary.NetCore.Lis.v2;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using StudentsAccessor;

namespace AssignmentsManager
{
    public static class MembersAccessor
    {
        [FunctionName("GetMembers")]
        public static async Task<IActionResult> GetAssignmentMembers(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "assignments/{guid}/members")] HttpRequest req,
            [Table("Assignments", Connection = "StorageConnection")] CloudTable assignments,
            string guid,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            if (!Guid.TryParse(guid, out var queryQuid))
            {
                return new BadRequestErrorMessageResult("Invalid Guid");
            }
            var query = new TableQuery<Assignment>().Where(
                TableQuery.GenerateFilterConditionForGuid("Guid", QueryComparisons.Equal, queryQuid));
            var assignmentQueryResult = await AssignmentAccessor.GetAssignmentsAsync(assignments, query);
            var assignment = assignmentQueryResult.FirstOrDefault();
            if (assignment == null)
            {
                return new NotFoundObjectResult($"Assignment with Guid {guid} was not found");
            }
            var membershipsManager = new LmsMemberships();
            var members = new List<Membership>();
            if (ValidateData(assignment))
            {
                members = await membershipsManager.GetMemberships(assignment.CustomContextMembershipsUrl,
                    assignment.OAuthConsumerKey, "secret", assignment.ResourceLinkId);
            }

            return new OkObjectResult(members);
        }

        [FunctionName("GetMember")]
        public static async Task<IActionResult> GetAssignmentMember(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "assignments/{guid}/members/{id}")] HttpRequest req,
            [Table("Assignments", Connection = "StorageConnection")] CloudTable assignments,
            string guid,
            string id,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var assignmentMembers = (ObjectResult)await GetAssignmentMembers(req, assignments, guid, log);
            var memberships = (List<Membership>)assignmentMembers.Value;
            var user = memberships.Where(member => member.Member.UserId == id);
            if (!user.Any())
            {
                return new NotFoundObjectResult($"User with Id {id} in Assignment with Guid {guid} was not found");
            }

            return new OkObjectResult(user);
        }

        private static bool ValidateData(Assignment assignment)
        {
            return !string.IsNullOrEmpty(assignment.CustomContextMembershipsUrl) &&
                   !string.IsNullOrEmpty(assignment.OAuthConsumerKey) &&
                   !string.IsNullOrEmpty(assignment.ResourceLinkId);
        }
    }
}
