using System.Collections.Generic;
using System.IO;
using System.Net;

namespace OAuth2Provider.Request
{
    public class OAuthRequest : IOAuthMessage
    {
        public string LocationUri { get; set; }
        public string Body { get; set; }
        public IDictionary<string, string> Headers { get; set; }
        public string ContentType { get; set; }
        public string Method { get; set; }

        public HttpWebRequest AsWebRequest()
        {
            var request = (HttpWebRequest)WebRequest.Create(LocationUri);
            request.Method = Method;
            request.ContentType = ContentType;
            request.Method = Method;

            foreach (var header in Headers)
                request.Headers.Add(header.Key, header.Value);

            if (!string.IsNullOrWhiteSpace(Body))
            {
                using (var sw = new StreamWriter(request.GetRequestStream()))
                {
                    sw.Write(Body);
                }
            }

            return request;
        }

        public static RequestBuilder PasswordFlowTokenRequest(string clientId, string clientSecret, string username, string password)
        {
            return new RequestBuilder()
                        .SetContentType(OAuth2Provider.ContentType.FormEncoded)
                        .SetMethod(HttpMethod.Post)
                        .SetParam(OAuthTokens.ClientId, clientId)
                        .SetBasicAuthentication(clientId, clientSecret)
                        .SetParam(OAuthTokens.Username, username)
                        .SetParam(OAuthTokens.Password, password)
                        .SetParam(OAuthTokens.GrantType, GrantType.Password);
                        
        }

        public static RequestBuilder RefreshTokenRequest(string clientId, string clientSecret, string refreshToken)
        {
            return new RequestBuilder()
                            .SetParam(OAuthTokens.GrantType, GrantType.RefreshToken)
                            .SetContentType(OAuth2Provider.ContentType.FormEncoded)
                            .SetMethod(HttpMethod.Post)
                            .SetBasicAuthentication(clientId, clientSecret)
                            .SetParam(OAuthTokens.RefreshToken, refreshToken) ;
        }

        public static RequestBuilder ClientCredentialsTokenRequest(string clientId, string clientSecret)
        {
            return new RequestBuilder()
                .SetContentType(OAuth2Provider.ContentType.FormEncoded)
                .SetMethod(HttpMethod.Post)
                .SetBasicAuthentication(clientId, clientSecret)
                .SetParam(OAuthTokens.GrantType, GrantType.ClientCredentials);
        }

        public static RequestBuilder ResourceRequest(string uri, string accessToken)
        {
            return new RequestBuilder()
                    .SetLocation(uri)
                    .SetAccessToken(accessToken);
        }
    }
}
