using Microsoft.WindowsAzure.Storage.Table;

namespace AssignmentsManager
{
    internal class LmsAssignment : TableEntity
    {
        public string OutcomeServiceUrl { get; set; }
        public string CustomContextMembershipsUrl { get; set; }
        public string OAuthConsumerKey { get; set; }
        public string ResourceLinkId { get; set; }
        public string ResultSourcedId { get; set; }
        public string LmsProviderName { get; set; }

        public LmsAssignment()
        {
            
        }
    }
}
