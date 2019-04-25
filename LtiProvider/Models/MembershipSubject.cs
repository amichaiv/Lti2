using System.Collections.Generic;
using Common;

namespace LtiProvider.Models
{
    public class MembershipSubject
    {
        public string Type { get; set; }
        public string ContextId { get; set; }
        public List<Membership> Membership { get; set; }
    }
}