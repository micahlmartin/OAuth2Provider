using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth2Provider.Request
{
    public interface IRequest
    {
        IDictionary<string, IList<string>> Values { get; }
        IDictionary<string, IList<string>> Headers { get; }
        string ContentType { get; }
        string HttpMethod { get; }
        bool IsSecure { get; }
    }
}
