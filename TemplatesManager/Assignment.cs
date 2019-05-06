using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace TemplatesManager
{
    class Assignment: TableEntity
    {
        public Guid Guid { get; set; }
        public string OutcomeServiceUrl { get; set; }
        public string ResultSourcedId { get; set; }
        public string CustomContextMembershipsUrl { get; set; }
        public string OAuthConsumerKey { get; set; }
        public string ResourceLinkId { get; set; }
        public string CourseName { get; set; }
        public string LmsName { get; set; }
        public string LtiName { get; set; }
    }
}
