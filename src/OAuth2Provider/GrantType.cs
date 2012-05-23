using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth2Provider
{
    public static class GrantType
    {
        public const string AuthorizationCode = "authorization_code";
        public const string Password = "password";
        public const string ClientCredentials = "client_credentials";
        public const string RefreshToken = "refresh_token";
        public const string Anonymous = "anon";
        public const string Known = "known";
    }
}
