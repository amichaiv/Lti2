using System;

namespace Common
{
    public class LtiLaunchRequestData 
    {
        public Guid Id { get; set; }
        public string OutcomeServiceUrl { get; set; }
        public string ResultSourcedId { get; set; }
        public string CustomContextMembershipsUrl { get; set; }
        public string OAuthConsumerKey { get; set; }
        public string ResourceLinkId { get; set; }
        public string ResourceLinkTitle { get; set; }
        public ContextRole Role { get; set; }
        public string ContextTitle { get; set; }
    }
}