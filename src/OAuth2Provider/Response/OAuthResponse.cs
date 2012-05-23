using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth2Provider.Response
{
    public class OAuthResponse : IOAuthMessage
    {
        public string LocationUri { get; set; }
        public string Body { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public int StatusCode { get; set; }
        public string ContentType { get; set; }

        public static ResponseBuilder TokenResponse(string accessToken, int expiresIn, string refreshToken)
        {
            return new ResponseBuilder()
                .DisableCache()
                .SetStatusCode(200)
                .SetParam(OAuthTokens.AccessToken, accessToken)
                .SetParam(OAuthTokens.ExpiresIn, expiresIn);
        }

        public static ResponseBuilder AuthorizationCodeResponse(string authorizationToken, int expiresIn, string redirectUri)
        {
            return new ResponseBuilder()
                .DisableCache()
                .SetStatusCode(302)
                .SetLocation(redirectUri)
                .SetParam(OAuthTokens.Code, authorizationToken)
                .SetParam(OAuthTokens.TokenType, "bearer")
                .SetParam(OAuthTokens.ExpiresIn, expiresIn);
        }
    }
}
