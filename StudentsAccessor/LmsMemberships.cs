using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LtiLibrary.NetCore.Clients;
using LtiLibrary.NetCore.Lis.v2;

namespace StudentsAccessor
{
    public class LmsMemberships
    {
        public async Task<IEnumerable<Member>> GetMemberships(
            string contextMembershipsUrl, string oAuthConsumerKey,
            string customerSecret, string resourceLinkId)
        {
            if (!ValidateData(contextMembershipsUrl, oAuthConsumerKey, customerSecret, resourceLinkId))
                return new List<Member>();
            using (var client = new HttpClient())
            {
                var clientResponse =
                    await MembershipClient.GetMembershipAsync(client, contextMembershipsUrl,
                        oAuthConsumerKey, customerSecret, resourceLinkId);
                return clientResponse.Response.Select(MembershipToMemberMapper);
            }
        }

        private static bool ValidateData(string contextMembershipsUrl, string oAuthConsumerKey,
            string customerSecret, string resourceLinkId)
        {
            return !string.IsNullOrEmpty(contextMembershipsUrl)
                   && !string.IsNullOrEmpty(oAuthConsumerKey)
                   && !string.IsNullOrEmpty(customerSecret)
                   && !string.IsNullOrEmpty(resourceLinkId);
        }

        private static Member MembershipToMemberMapper(Membership membership)
        {
            return new Member
            {
                UserId = membership.Member.UserId,
                FamilyName = membership.Member.FamilyName,
                GivenName = membership.Member.GivenName,
                Email = membership.Member.Email,
                Role = membership.Role.Select(role=> role.ToString()),
                Status = membership.Status.ToString()
            };
        }
    }
}