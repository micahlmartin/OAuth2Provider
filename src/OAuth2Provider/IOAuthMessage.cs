using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth2Provider
{
    public interface IOAuthMessage
    {
        string LocationUri { get; set; }
        string Body { get; set; }
        IDictionary<string, string> Headers { get; set; }
        string ContentType { get; set; }
    }
}
