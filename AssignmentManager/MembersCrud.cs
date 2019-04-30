using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using StudentsAccessor;

namespace AssignmentsManager
{
    public static class MembersCrud
    {
        [FunctionName("MembersCrud")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Table("Assignments", Connection = "StorageConnection")] CloudTable assignments,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var entities = await AssignmentAccessor.GetAssignmentsAsync(assignments);
            var membershipsManager = new LmsMemberships();
            // Need to get members according to some param in req
            // Temp solution
            var data = entities.First();
            var members = await membershipsManager.GetMemberships(data.CustomContextMembershipsUrl,
                data.OAuthConsumerKey, "secret", data.ResourceLinkId);
            return new OkObjectResult(members);
        }
    }
}
