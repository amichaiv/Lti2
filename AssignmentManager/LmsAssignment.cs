using Microsoft.WindowsAzure.Storage.Table;

namespace AssignmentsManager
{
    internal class LmsAssignment : Assignment
    {
        public string OutcomeServiceUrl { get; set; }
        public string CustomContextMembershipsUrl { get; set; }
        public string OAuthConsumerKey { get; set; }
        public string ResourceLinkId { get; set; }
        public string ResultSourcedId { get; set; }

        public LmsAssignment()
        {
            
        }

        public LmsAssignment(string partitionKey, string contextId, string resourceLinkId)
        {
            PartitionKey = partitionKey;
            RowKey = $"{contextId}_{resourceLinkId}";
        }
    }
}
