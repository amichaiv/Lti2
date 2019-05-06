using Microsoft.WindowsAzure.Storage.Table;

namespace AssignmentsManager
{
    class LmsAssignment : TableEntity
    {
        public string CustomContextMembershipsUrl { get; set; }
        public string ResourceLinkId { get; set; }
        public string OAuthConsumerKey { get; set; }
    }
}
