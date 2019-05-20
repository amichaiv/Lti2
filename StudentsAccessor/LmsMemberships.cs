using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LtiLibrary.NetCore.Clients;
using LtiLibrary.NetCore.Lis.v2;
using Newtonsoft.Json.Linq;

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

        public async Task<List<Member>> GetMembershipsWithOAuth(string membershipsUrl, string accessToken)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                var response = await client.GetAsync(new Uri(membershipsUrl));
                var result = await response.Content.ReadAsStringAsync();
                //if (result == ExpiredAccessToken)
                //    throw new ArgumentException("Access Token Expired");
                var jToken = JObject.Parse(result);
                var memberships = jToken["pageOf"]["membershipSubject"]["membership"].ToArray();
                return ParseJTokenArrayToMemberships(memberships);
            }
        }

        private static List<Member> ParseJTokenArrayToMemberships(JToken[] memberships)
        {
            return (from membership in memberships
                let member = membership["member"]
                select new Member
                {
                    GivenName = member["givenName"].ToString(),
                    FamilyName = member["familyName"].ToString(),
                    UserId = member["userId"].ToString(),
                    Email = member["email"].ToString(),
                    Role = membership["role"].Select(role => role.ToString()),
                    Status = membership["status"].ToString()
                }).ToList();
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