using Microsoft.WindowsAzure.Storage.Table;

namespace AssignmentsManager.Models
{
    internal class LmsAssignment : TableEntity
    {
        public string OutcomeServiceUrl { get; set; }
        public string CustomContextMembershipsUrl { get; set; }
        public string OAuthConsumerKey { get; set; }
        public string ResourceLinkId { get; set; }
        public string ResultSourcedId { get; set; }

        public LmsAssignment()
        {
            
        }
    }
}
