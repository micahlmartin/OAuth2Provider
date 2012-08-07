using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using OAuth2Provider.Request;
using OAuth2Provider;

namespace OAuth2Provider.Request
{
    public class HttpRequest : IRequest
    {
        private HttpRequestBase _request;
        private IDictionary<string, IList<string>> _headers;
        private IDictionary<string, IList<string>> _values;

        public HttpRequest(HttpRequestBase request)
        {
            _request = request;
        }

        public IDictionary<string, IList<string>> Values
        {
            get
            {
                if (_values == null)
                {
                    _values = new Dictionary<string, IList<string>>();
                    var formProps = _request.Form.ToDictionary().Select(x => new KeyValuePair<string, IList<string>>(x.Key, x.Value.Split(new[]{','}, StringSplitOptions.RemoveEmptyEntries)));
                    _values.AddRange(formProps);

                    var queryProps = _request.QueryString.ToDictionary().Select(x => new KeyValuePair<string, IList<string>>(x.Key, x.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)));
                    _values.SafeAddRange(queryProps);
                }

                return _values;
            }
        }

        public IDictionary<string, IList<string>> Headers
        {
            get 
            {
                if(_headers == null)
                {
                    _headers = new Dictionary<string, IList<string>>();
                    var headers = _request.Headers.ToDictionary().Select(x => new KeyValuePair<string, IList<string>>(x.Key, x.Value.Split(new[]{','}, StringSplitOptions.RemoveEmptyEntries).ToList()));
                    foreach(var header in headers)
                        _headers.Add(header);
                }

                return _headers;
            }
        }

        public string ContentType
        {
            get {  return _request.ContentType; }
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
