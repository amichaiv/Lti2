using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AuthorizationManager
{
    public static class LoginOAuth
    {
        [FunctionName("AuthorizeCredentials")]
        public static async Task<IActionResult> AuthorizeCredentials(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "login/oath2/credentials")] HttpRequest req,
            [Table("Authentication", Connection = "StorageConnection")] CloudTable authenticationTable,
            ILogger log)
        {
            string clientId = req.Query["client_id"];
            string clientSecret = req.Query["client_secret"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            clientId = clientId ?? data?.client_id;
            clientSecret = clientSecret ?? data?.client_secret;
            var redirectUrl = Environment.GetEnvironmentVariable("OAuthTokenRedirectUrl");
            var authEntity = new OAuthEntity
            {
                ClientId = clientId,
                PartitionKey = clientId,
                RowKey = clientId,
                ClientSecret = clientSecret,
                RedirectUri = redirectUrl
            };
            await AddAsync(authenticationTable, authEntity);
            var fullRedirectUrl = Authentication.GetAuthenticationUrl(OAuthEntity.BaseAuthUrl, clientId, redirectUrl);
            log.LogInformation($"Redirect to {fullRedirectUrl}");
            return new RedirectResult(fullRedirectUrl, true);
        }

        [FunctionName("GenerateAccessToken")]
        public static async Task<IActionResult> GenerateAccessToken(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "login/oauth2/token")] HttpRequest req,
            [Table("Authentication", Connection = "StorageConnection")] CloudTable authenticationTable,
            ILogger log)
        {
            string code = req.Query["code"];
            var authEntity = await GetAsync(authenticationTable);
            if (authEntity == null)
            {
                log.LogWarning("Couldn't find any stored authentication data");
                return new NotFoundResult();
            }

            await GenerateAccessToken(authenticationTable, authEntity, code);
            return new OkObjectResult("Access token generated successfully");
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

        private static async Task GenerateAccessToken(CloudTable authenticationTable, OAuthEntity authEntity, string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                await RefreshAccessToken(authenticationTable, authEntity);
                return;
            }
            var ltiAuthentication = new Authentication(authEntity);
            var request = ltiAuthentication.GetAuthorizationTokenRequest(code);
            HttpContent content = new StringContent(request, Encoding.UTF8, "application/json");
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(Authentication.OAuthTokenUrl, content);
                if (!response.IsSuccessStatusCode)
                {
                    await RefreshAccessToken(authenticationTable, authEntity);
                    return;
                }
                var responseContent = await response.Content.ReadAsStringAsync();
                var jToken = JObject.Parse(responseContent);
                var auth = new OAuthEntity
                {
                    AccessToken = jToken["access_token"].ToString(),
                    RefreshToken = jToken["refresh_token"].ToString()
                };
                await AddAsync(authenticationTable, auth);
            }
        }

        private static async Task RefreshAccessToken(CloudTable authenticationTable, OAuthEntity authEntity)
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



    }
}
