using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth2Provider
{
    public static class OAuthTokens
    {
        public const string GrantType = "grant_type";
        public const string Username = "username";
        public const string Password = "password";
        public const string Scope = "scope";
        public const string AccessToken = "access_token";
        public const string OAuthToken = "oauth_token";
        public const string TokenType = "token_type";
        public const string ExpiresIn = "expires_in";
        public const string RefreshToken = "refresh_token";
        public const string ClientId = "client_id";
        public const string ClientSecret = "client_secret";
        public const string ResponseType = "response_type";
        public const string RedirectUri = "redirect_uri";
        public const string HeaderName = "OAuth";
        public const string Error = "error";
        public const string ErrorDescription = "error_description";
        public const string ErrorUri = "error_uri";
        public const string Code = "code";
        public const string State = "state";
    }

    public static class ContentType
    {
        public const string Json = "application/json";
        public const string FormEncoded = "application/x-www-form-urlencoded";
    }

    public static class HttpMethod
    {
        public const string Post = "POST";
        public const string Get = "GET";
        public const string Put = "PUT";
        public const string Delete = "Delete";
    }

    public static class WWWAuthHeader
    {
        public const string REALM = "realm";
    }

    public static class HeaderType
    {
        public const string ContentType = "Content-Type";
        public const string WWWAuthenticate = "WWW-Authenticate";
        public const string Authorization = "Authorization";
        public const string CacheControl = "Cache-Control";
    }
}
