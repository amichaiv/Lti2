using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using StudentsAccessor;

namespace AssignmentsManager
{
    public static class AssignmentMembersApi
    {
        [FunctionName("GetAssignmentMembers")]
        public static async Task<IActionResult> GetAssignmentMembers(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "assignments/{assignmentGuid}/members")] HttpRequest req,
            [Table("Assignments", Connection = "StorageConnection")] CloudTable assignments,
            string assignmentGuid,
            ILogger log)
        {
            if (!Guid.TryParse(assignmentGuid, out var queryQuid))
            {
                return new BadRequestErrorMessageResult($"Invalid assignment Guid {assignmentGuid}");
            }
            var lmsAssignment = await GetLmsAssignment(assignments, queryQuid);
            if (lmsAssignment == null)
            {
                return new NotFoundObjectResult($"Assignment with Guid {assignmentGuid} was not found");
            }
            var lmsMembershipsAccessor = new LmsMemberships();
            var members = await lmsMembershipsAccessor.GetMemberships(lmsAssignment.CustomContextMembershipsUrl,
                lmsAssignment.OAuthConsumerKey, "secret", lmsAssignment.ResourceLinkId);

            return new OkObjectResult(members);

        }

        [FunctionName("GetAssignmentMember")]
        public static async Task<IActionResult> GetAssignmentMember(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "assignments/{assignmentGuid}/members/{memberId}")] HttpRequest req,
            [Table("Assignments", Connection = "StorageConnection")] CloudTable assignments,
            string assignmentGuid,
            string memberId,
            ILogger log)
        {
            if (!Guid.TryParse(assignmentGuid, out var queryQuid))
            {
                return new BadRequestErrorMessageResult($"Invalid assignment Guid {assignmentGuid}");
            }
            var lmsAssignment = await GetLmsAssignment(assignments, queryQuid);
            if (lmsAssignment == null)
            {
                return new NotFoundObjectResult($"Assignment with Guid {assignmentGuid} was not found");
            }
            var membershipsManager = new LmsMemberships();
            var members = await membershipsManager.GetMemberships(lmsAssignment.CustomContextMembershipsUrl,
                lmsAssignment.OAuthConsumerKey, "secret", lmsAssignment.ResourceLinkId);

            var user = members.FirstOrDefault(member => member.Member.UserId == memberId);
            if (user == null)
            {
                return new NotFoundObjectResult($"User with Id {memberId} in Assignment with Guid {assignmentGuid} was not found");
            }

            return new OkObjectResult(user);
        }

        private static async Task<LmsAssignment> GetLmsAssignment(CloudTable assignments, Guid guid)
        {
            var filter = TableQuery
                .GenerateFilterConditionForGuid("Guid", QueryComparisons.Equal, guid);

            var query = new TableQuery<LmsAssignment>().Where(filter).Take(1);
            var queryResult = await assignments.ExecuteQuerySegmentedAsync(query, null);
            return queryResult.Results.FirstOrDefault();
        }
    }
}
