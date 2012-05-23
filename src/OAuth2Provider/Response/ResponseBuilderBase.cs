using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OAuth2Provider.MessageWriters;

namespace OAuth2Provider.Response
{
    public abstract class ResponseBuilderBase
    {
        protected int _statusCode;
        protected IDictionary<string, object> _parameters = new Dictionary<string, object>();
        protected IDictionary<string, string> _headers = new Dictionary<string, string>();
        protected string Location { get; set; }
        protected int StatusCode { get; set; }
        protected IDictionary<string, object> Parameters
        {
            get { return _parameters; }
        }
        protected IDictionary<string,string> Headers
        {
            get { return _headers; }
        }


        public OAuthResponse BuildQueryMessage()
        {
            var message = GetBaseMessage();
            var writer = new QueryStringOAuthMessageWriter();
            writer.Write(message, _parameters);
            return message;
        }

        public OAuthResponse BuildBodyMessage()
        {
            var message = GetBaseMessage();
            message.ContentType = ContentType.FormEncoded;
            var writer = new UrlEncodedBodyOAuthMessageWriter();
            writer.Write(message, _parameters);
            return message;
        }

        public OAuthResponse BuildJsonMessage()
        {
            var message = GetBaseMessage();
            message.ContentType = ContentType.Json;
            var writer = new JsonOAuthMessageWriter();
            writer.Write(message, _parameters);
            return message;
        }

        private OAuthResponse GetBaseMessage()
        {
            return new OAuthResponse { LocationUri = Location, StatusCode = StatusCode, Headers = Headers };
        }
    }
}
