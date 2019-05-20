using Microsoft.WindowsAzure.Storage.Table;

namespace AuthorizationManager
{
    public class OAuthEntity : TableEntity
    {
        public const string BaseAuthUrl = "http://13.93.45.80/login/oauth2/auth";
        public const string BaseOAuthTokenUrl = "http://13.93.45.80/login/oauth2/token";
        public const string GrantTypeAuthorization = "authorization_code";
        public const string GrantTypeRefreshToken = "refresh_token";
        public const string OAuthTokenUrl = "http://13.93.45.80/login/oauth2/token";

        public string RedirectUri = "https://localhost:5001/api/accesstoken";
        public string ClientId { get; set; } = "10000000000003";
        public string ClientSecret { get; set; } = "isSAyOhS191kD4ya31CKMEbFMR9fiDjh91ByTGw1PaJi0fAxLwo6Gx4hCWMjT3kT";
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}