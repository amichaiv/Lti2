using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using LtiLibrary.NetCore.Clients;
using LtiLibrary.NetCore.Lis.v2;

namespace MembershipsAccessor
{
    public class LmsMemberships
    {
        public async Task<List<Membership>> GetMemberships(
            string contextMembershipsUrl, string oAuthConsumerKey,
            string customerSecret, string resourceLinkId)
        {
            if (!ValidateData(contextMembershipsUrl, oAuthConsumerKey, customerSecret, resourceLinkId))
                return new List<Membership>();
            using (var client = new HttpClient())
            {
                var clientResponse =
                    await MembershipClient.GetMembershipAsync(client, contextMembershipsUrl,
                        oAuthConsumerKey, customerSecret, resourceLinkId);
                return clientResponse.Response;
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
    }
}