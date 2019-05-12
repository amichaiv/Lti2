using System.Collections.Generic;

namespace AssignmentsManager
{
    public class Member
    {
        public string UserId { get; set; }
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public IEnumerable<string> Role;
    }
}