using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth2Provider.Response
{
    public class ResponseBuilder : ResponseBuilderBase
    {
        public ResponseBuilder SetStatusCode(int statusCode)
        {
            StatusCode = statusCode;
            return this;
        }

        public ResponseBuilder SetLocation(string location)
        {
            Location = location;
            return this;
        }

        public ResponseBuilder SetParam(string key, object value)
        {
            Parameters.Add(key, value);
            return this;
        }

        public ResponseBuilder SetHeader(string key, string value)
        {
            Headers.Add(key, value);
            return this;
        }

        public ResponseBuilder DisableCache()
        {
            Headers.Add("Cache-Control", "no-store");
            return this;
        }
    }
}
