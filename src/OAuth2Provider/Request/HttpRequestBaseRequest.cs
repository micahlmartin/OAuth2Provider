using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace OAuth2Provider.Request
{
    public class HttpRequestBaseRequest : IRequest
    {
        private readonly HttpRequestBase _request;
        private IDictionary<string, string> _properties;
 
        public HttpRequestBaseRequest(HttpRequestBase request)
        {
            _request = request;
        }

        public IDictionary<string, string> Properties
        {
            get
            {
                if(_properties == null)
                {
                    _properties = new Dictionary<string, string>();
                    foreach(KeyValuePair<string, string> kvp in _request.Form.ToDictionary())
                        _properties.Add(kvp);

                    foreach (KeyValuePair<string, string> kvp in _request.QueryString.ToDictionary())
                        _properties.Add(kvp);

                    foreach (KeyValuePair<string, string> kvp in _request.Cookies.ToDictionary())
                        _properties.Add(kvp);
                }

                return _properties;
            }
        }
        public IDictionary<string, string> Headers
        {
            get { return _request.Headers.ToDictionary(); }
        }
        public string ContentType
        {
            get { return _request.ContentType; }
        }
        public string HttpMethod
        {
            get { return _request.HttpMethod; }
        }
        public bool IsSecure
        {
            get { return _request.IsSecureConnection; }
        }
    }
}
