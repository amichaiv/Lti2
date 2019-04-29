using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Common;
using LtiLibrary.NetCore.Clients;
using LtiProvider.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace LtiProvider
{
    public class MembershipsManager
    {
    //    public async Task<IEnumerable<Membership>> GetMemberships(HttpRequest httpRequest)
    //    {
    //        using (var client = new HttpClient())
    //        {
    //            var clientResponse =
    //                await MembershipClient.GetMembershipAsync(client, data.CustomContextMembershipsUrl,
    //                    data.OAuthConsumerKey, "secret", data.ResourceLinkId);
    //            return GetMembershipsFromResponse(data, clientResponse.HttpResponse);
    //        }
    //    }

    //    private static IEnumerable<Membership> GetMembershipsFromResponse(LtiLaunchRequestData data, string response)
    //    {
    //        var responseArray = response.Split(Environment.NewLine);
    //        var jsonElement = responseArray[responseArray.Length - 1];
    //        var jObject = JObject.Parse(jsonElement);
    //        var rootObject = jObject.ToObject<RootObject>();
    //        var membershipSubject = rootObject.PageOf.MembershipSubject;
    //        return
    //            membershipSubject.Membership;
    //    }


    }
}
