using System.Collections.Generic;
using OAuth2Provider.MessageWriters;

namespace OAuth2Provider.Request
{
    public class RequestBuilder
    {
        private readonly IDictionary<string, string> _headers = new Dictionary<string, string>();
        private readonly IDictionary<string, object> _params = new Dictionary<string, object>();
        private string _location;
        private string _method;
        private string _contentType;

        public RequestBuilder SetContentType(string contentType)
        {
            _contentType = contentType;
            return this;
        }

        public RequestBuilder SetLocation(string url)
        {
            _location = url;
            return this;
        }

        public RequestBuilder SetMethod(string method)
        {
            _method = method;
            return this;
        }

        public RequestBuilder SetHeader(string name, object value)
        {
            _headers.Add(name, value.ToString());
            return this;
        }

        public RequestBuilder SetBasicAuthentication(string username, string password)
        {
            _headers.Add(HeaderType.Authorization, "Basic " + new HttpBasicAuthenticationScheme(username, password).ToString());

            return this;
        }

        public RequestBuilder SetAccessToken(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return this;

            _headers.Add(HeaderType.Authorization, "Bearer " + value);
            return this;
        }

        public RequestBuilder SetParam(string name, object val)
        {
            _params.Add(name, val.ToString());
            return this;
        }

        public RequestBuilder SetScope(string scope)
        {
            _params.Add(OAuthTokens.Scope, scope);
            return this;
        }

        public OAuthRequest BuildQueryMessage()
        {
            var message = GetBaseMessage();
            var writer = new QueryStringOAuthMessageWriter();
            writer.Write(message, _params);
            return message;
        }

        public OAuthRequest BuildBodyMessage()
        {
            var message = GetBaseMessage();
            message.ContentType = ContentType.FormEncoded;
            var writer = new UrlEncodedBodyOAuthMessageWriter();
            writer.Write(message, _params);
            return message;
        }

        public OAuthRequest BuildJsonMessage()
        {
            var message = GetBaseMessage();
            message.ContentType = ContentType.Json;
            var writer = new JsonOAuthMessageWriter();
            writer.Write(message, _params);
            return message;
        }

        private OAuthRequest GetBaseMessage()
        {
            return new OAuthRequest { LocationUri = _location, Headers = _headers, Method = _method, ContentType = _contentType };
        }
    }
}
