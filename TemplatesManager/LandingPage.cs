using System;
using System.Linq;
using System.Threading.Tasks;
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
            Assignment assignmentToDb = ParseToAssignment(ltiRequest);
            Assignment assignment = await GetOrAddToDbAsync(assignments, assignmentToDb);

            var urlWithParams = $"{RedirectUrl}/en?assignmentGuid={assignment.Guid}&userId={ltiRequest.UserId}";
            log.LogInformation($"Redirect to {urlWithParams}");
            return new RedirectResult(urlWithParams, true);
        }

        private static Assignment ParseToAssignment(LtiRequest ltiRequest)
        {
            var ltiRequestCustomParameters = ltiRequest.CustomParameters;
            var customParams = ltiRequestCustomParameters.Split('&');
            var membershipsUrlStatement = customParams.FirstOrDefault(param => param.Contains("custom_context_memberships_url"));
            var membershipsValue = membershipsUrlStatement?.Split('=')[1];
            
            Assignment assignment = new Assignment
            {
                CourseName = ltiRequest.ContextTitle,
                LmsName = ltiRequest.ToolConsumerInstanceName,
                LtiName = ltiRequest.ResourceLinkTitle,
                OutcomeServiceUrl = ltiRequest.LisOutcomeServiceUrl,
                ResultSourcedId = ltiRequest.LisResultSourcedId,
                CustomContextMembershipsUrl = membershipsValue,
                OAuthConsumerKey = ltiRequest.ConsumerKey,
                ResourceLinkId = ltiRequest.ResourceLinkId,
                PartitionKey = $"{ltiRequest.ToolConsumerInstanceName}",
                RowKey = $"{ltiRequest.ContextId}_{ltiRequest.ResourceLinkId}"
            };

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

            assignment.Guid = Guid.NewGuid();
            
            TableOperation tableOperation = TableOperation.InsertOrMerge(assignment);
            await assignments.ExecuteAsync(tableOperation);
            return assignment;
        }
    }
}
