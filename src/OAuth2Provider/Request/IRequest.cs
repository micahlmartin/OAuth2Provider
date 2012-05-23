using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth2Provider.Request
{
    public interface IRequest
    {
        IDictionary<string,string> Properties { get; }
        IDictionary<string, string> Headers { get; }
        string ContentType { get; }
        string HttpMethod { get; }
        bool IsSecure { get; }
    }
}
