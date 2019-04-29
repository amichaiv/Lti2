using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using LtiLibrary.NetCore.Clients;
using LtiLibrary.NetCore.Lis.v2;

namespace StudentsAccessor
{
    public class LmsMemberships
    {
        public async Task<List<Membership>> GetMemberships(
            string contextMembershipsUrl, string oAuthConsumerKey, 
            string customerSecret, string resourceLinkId)
        {
            using (var client = new HttpClient())
            {
                var clientResponse =
                    await MembershipClient.GetMembershipAsync(client, contextMembershipsUrl,
                        oAuthConsumerKey, customerSecret, resourceLinkId);
                return clientResponse.Response;
            }
        }
    }
}
