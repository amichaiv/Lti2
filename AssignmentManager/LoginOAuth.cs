using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json.Linq;

namespace AssignmentsManager
{
    public static class LoginOAuth
    {
        public static async Task RefreshAccessToken(CloudTable authenticationTable, OAuthEntity authEntity)
        {
            var ltiAuthentication = new Authentication(authEntity);
            var request = ltiAuthentication.GetRefreshTokenRequest(authEntity.RefreshToken);
            var content = new StringContent(request, Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(Authentication.OAuthTokenUrl, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                var jToken = JObject.Parse(responseContent);
                authEntity.AccessToken = jToken["access_token"].ToString();
                await AddAsync(authenticationTable, authEntity);
            }
        }

        public static async Task<OAuthEntity> GetAsync(CloudTable authenticationTable)
        {
            var query = new TableQuery<OAuthEntity>().Take(1);
            var queryResult = await authenticationTable.ExecuteQuerySegmentedAsync(query, null);
            return queryResult.Results.FirstOrDefault();
        }

        private static async Task<OAuthEntity> AddAsync(CloudTable authenticationTable, OAuthEntity authentication)
        {
            var result = await GetEntityAsync(authenticationTable, authentication);
            if (result != null)
            {
                return authentication;
            }
            TableOperation tableOperation = TableOperation.InsertOrMerge(authentication);
            var executeResult = await authenticationTable.ExecuteAsync(tableOperation);
            var storedAuth = (OAuthEntity)executeResult.Result;
            return storedAuth;
        }

        private static async Task<OAuthEntity> GetEntityAsync(CloudTable authenticationTable, OAuthEntity authentication)
        {
            var retrieveOperation = TableOperation.Retrieve<OAuthEntity>(authentication.PartitionKey, authentication.RowKey);
            var tableResult = await authenticationTable.ExecuteAsync(retrieveOperation);
            if (tableResult.Result is OAuthEntity result)
                return result;
            return null;
        }


    }
}
