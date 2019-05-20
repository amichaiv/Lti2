using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace TemplatesManager
{
    internal class Assignment: TableEntity
    {
        public Guid Guid { get; set; }
        public string OutcomeServiceUrl { get; set; }
        public string ResultSourcedId { get; set; }
        public string CustomContextMembershipsUrl { get; set; }
        public string OAuthConsumerKey { get; set; }
        public string ResourceLinkId { get; set; }
        public string ResourceLinkTitle { get; set; }
        public string ResourceLinkDescription { get; set; }
        public string CourseName { get; set; }
        public string LmsInstanceName { get; set; }
        public string LmsProviderName { get; set; }
        public string LtiName { get; set; }
    }
}
