using System;
using LtiLibrary.NetCore.Lti.v1;
using Microsoft.WindowsAzure.Storage.Table;

namespace TemplatesManager
{
    public class Assignment : TableEntity
    {
        public Guid Guid { get; set; }
        public LtiRequest LtiRequest { get; }

        public Assignment(LtiRequest ltiRequest)
        {
            LtiRequest = ltiRequest;
            PartitionKey = $"{ltiRequest.ToolConsumerInstanceName}";
            RowKey = $"{ltiRequest.ContextId}_{ltiRequest.ResourceLinkId}";
        }

        public void GenerateGuid()
        {
            Guid = Guid.NewGuid();
        }
    }
}
