using System.Collections.Generic;

namespace Common
{
    public class Membership 
    {
        public string Status { get; set; }
        public List<string> Roles { get; set; }
        public Member Member { get; set; }
    }
}