using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace OAuth2Provider.MessageWriters
{
    public class QueryStringOAuthMessageWriter : IOAuthMessageWriter
    {
        public void Write(IOAuthMessage message, IDictionary<string, object> parameters)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            if (parameters == null)
                return;

            var url = new Uri(message.LocationUri);

            var queryParams = HttpUtility.ParseQueryString(url.Query);

            foreach (var parameter in parameters)
            {
                if (parameter.Value != null && !string.IsNullOrWhiteSpace(parameter.Value.ToString()))
                    queryParams.Add(parameter.Key, parameter.Value.ToString());
            }

            var newUrl = string.Empty;

            if(!string.IsNullOrWhiteSpace(url.Query))
                newUrl = message.LocationUri.Replace(url.Query, ""); //remove querystring from old url
            else
                newUrl = url.OriginalString;
  
            newUrl += queryParams.ToQueryString(); //append new querystring

            message.LocationUri = newUrl;
        }
    }
}
