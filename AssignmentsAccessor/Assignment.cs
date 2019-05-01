using System;
using System.Collections.Generic;
using System.Linq;
using LtiLibrary.NetCore.Lti.v1;
using Microsoft.WindowsAzure.Storage.Table;
using StudentsAccessor;

namespace AssignmentsAccessor
{
    public class Assignment : TableEntity
    {
        public Guid Guid { get; set; }
        public StudentAssignment[] StudentsAssignments { get; set; }
        public int NoOfStudents { get; set; } = new Random().Next(1, 10);
        public int TotalConsumed { get; set; } = new Random().Next(1, 10);
        public int NoOfProjectGroups { get; set; } = new Random().Next(1, 10);
        public int NoOfTeachingAssistants { get; set; } = new Random().Next(1, 10);
        public string OutcomeServiceUrl { get; set; }
        public string ResultSourcedId { get; set; }
        public string CustomContextMembershipsUrl { get; set; }
        public string OAuthConsumerKey { get; set; }
        public string ResourceLinkId { get; set; }

        public IEnumerable<Person> Members { get; set; }

        public Assignment()
        {

        }

        public Assignment(LtiRequest ltiRequest)
        {
            var ltiRequestCustomParameters = ltiRequest.CustomParameters;
            var customParams = ltiRequestCustomParameters.Split('&');
            var membershipsUrlStatement = customParams.Single(param => param.Contains("custom_context_memberships_url"));
            var membershipsValue = membershipsUrlStatement.Split('=')[1];

            OutcomeServiceUrl = ltiRequest.LisOutcomeServiceUrl;
            ResultSourcedId = ltiRequest.LisResultSourcedId;
            CustomContextMembershipsUrl =membershipsValue;
            OAuthConsumerKey = ltiRequest.ConsumerKey;
            ResourceLinkId = ltiRequest.ResourceLinkId;
            PartitionKey = $"{ltiRequest.ToolConsumerInstanceName}";
            RowKey = $"{ltiRequest.ContextId}_{ltiRequest.ResourceLinkId}";
        }

        public void GenerateGuid()
        {
            Guid = Guid.NewGuid();
        }
    }
}
