using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace OAuth2Provider.MessageWriters
{
    public class UrlEncodedBodyOAuthMessageWriter : IOAuthMessageWriter
    {
        public void Write(IOAuthMessage message, IDictionary<string, object> parameters)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            if (parameters == null)
                message.Body = string.Empty;
            else
            {
                var body = parameters.ToQueryString();
                if (body.StartsWith("?"))
                    body = body.Substring(1);

                message.Body = body;
            }
        }
    }
}
