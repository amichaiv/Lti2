namespace TemplatesManager
{
    public class Authentication 
    {
        public const string BaseAuthUrl = "https://13.93.45.80/login/oauth2/auth";
        public const string BaseOAuthTokenUrl = "http://13.93.45.80/login/oauth2/token";
        public const string GrantTypeAuthorization = "authorization_code";
        public const string GrantTypeRefreshToken = "refresh_token";
        public const string OAuthTokenUrl = "http://13.93.45.80/login/oauth2/token";
        public string RedirectUri = "https://localhost:5001/api/accesstoken";

        public Authentication(OAuthEntity authEntity)
        {
            ClientId = authEntity.ClientId;
            ClientSecret = authEntity.ClientSecret;
        }
        
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public static string GetAuthenticationUrl(string baseAuthUrl, string clientId, string redirectUri)
            => $"{baseAuthUrl}?client_id={clientId}&response_type=code&redirect_uri={redirectUri}";

        public string GetAuthorizationTokenRequest(string code)
        {
            return
                "{\"grant_type\":\"authorization_code\","
                + $"\"client_id\":\"{ClientId}\","
                + $"\"client_secret\":\"{ClientSecret}\","
                + $"\"code\":\"{code}\","
                + $"\"redirect_uri\":\"{RedirectUri}\""
                + "}";
        }

        public string GetRefreshTokenRequest(string refreshToken)
        {
            return
                "{\"grant_type\":\"refresh_token\","
                + $"\"client_id\":\"{ClientId}\","
                + $"\"client_secret\":\"{ClientSecret}\","
                + $"\"refresh_token\":\"{refreshToken}\","
                + $"\"redirect_uri\":\"{RedirectUri}\""
                + "}";
        }
    }
}