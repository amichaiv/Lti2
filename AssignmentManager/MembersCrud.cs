using System.Threading.Tasks;
using AssignmentsAccessor;
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

            var membershipsManager = new LmsMemberships();
            var data = GetLtiLaunchRequestData(req);
            var members = await membershipsManager.GetMemberships(data.CustomContextMembershipsUrl,
                data.OAuthConsumerKey, "secret", data.ResourceLinkId);
            return new OkObjectResult(members);
        }

        private static LtiLaunchRequestData GetLtiLaunchRequestData(HttpRequest request)
        {
            //var form = request.Form;
            //form.TryGetValue("roles", out var roles);
            //if (!Enum.TryParse(roles.ToString(), out ContextRole contextRole))
            //{
            //    contextRole = ContextRole.Learner;
            //}
            //form.TryGetValue("context_title", out var contextTitle);
            //form.TryGetValue("resource_link_id", out var resourceLinkId);
            //form.TryGetValue("resource_link_title", out var resourceLinkTitle);
            //form.TryGetValue("custom_context_memberships_url", out var customContextMembershipsUrl);
            //form.TryGetValue("lis_outcome_service_url", out var outcomeServiceUrl);
            //form.TryGetValue("lis_result_sourcedid", out var lisResultSourceDid);
            //form.TryGetValue("oauth_consumer_key", out var oauthConsumerKey);
            return new LtiLaunchRequestData
            {
                OutcomeServiceUrl = "http://51.144.118.10/moodle/mod/lti/service.php",
                ResultSourcedId = "{\"data\":{\"instanceid\":\"3\",\"userid\":\"3\",\"typeid\":\"1\",\"launchid\":1011296317},\"hash\":\"b4b9dbfbff6e5436741e4b1fab6e203fd2d62f05edea956414cfefd5a114dc63\"}",
                OAuthConsumerKey = "consumer.key",
                CustomContextMembershipsUrl = "http://51.144.118.10/moodle/mod/lti/services.php/CourseSection/3/bindings/1/memberships",
                Role = ContextRole.Learner,
                ResourceLinkId = "3",
                ResourceLinkTitle = "Membership Service Tool",
                ContextTitle = "Microsoft Education Hub"
            };
        }
    }
}
