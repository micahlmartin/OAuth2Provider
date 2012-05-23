using System;
using System.Collections.Generic;

namespace OAuth2Provider.MessageWriters
{
    public class JsonOAuthMessageWriter : IOAuthMessageWriter
    {
        public void Write(IOAuthMessage message, IDictionary<string, object> parameters)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            if (parameters == null)
                message.Body = string.Empty;
            else
            {
                var cleanedParams = GetCleanedParameters(parameters);
                message.Body = cleanedParams.ToJson();
            }
        }

        private IDictionary<string, object> GetCleanedParameters(IDictionary<string, object> parameters)
        {
            var cleanedParams = new Dictionary<string, object>(parameters);

            foreach (var kvp in parameters)
            {
                if (kvp.Value == null)
                    cleanedParams.Remove(kvp.Key);
            }

            return cleanedParams;
        }
    }
}
