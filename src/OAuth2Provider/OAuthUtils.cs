using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace OAuth2Provider
{
    public static class OAuthUtils
    {
        private static string PARAMETER_SEPARATOR = "&";
        private static string NAME_VALUE_SEPARATOR = "=";

        public static String Format(this IEnumerable<KeyValuePair<string, string>> parameters)
        {
            var result = new StringBuilder();
            foreach (var parameter in parameters)
            {
                if (!string.IsNullOrWhiteSpace(parameter.Key) && !string.IsNullOrWhiteSpace(parameter.Value))
                {
                    string encodedName = HttpUtility.UrlEncode(parameter.Key);
                    string encodedValue = HttpUtility.UrlEncode(parameter.Value);

                    if (result.Length > 0)
                    {
                        result.Append(PARAMETER_SEPARATOR);
                    }
                    result.Append(encodedName);
                    result.Append(NAME_VALUE_SEPARATOR);
                    result.Append(encodedValue);
                }
            }
            return result.ToString();
        }

        public static string EncodeOAuthHeader(IDictionary<string, string> entries)
        {
            var sb = new StringBuilder();
            sb.Append(OAuthTokens.HeaderName).Append(" ");
            foreach (KeyValuePair<string, string> entry in entries)
            {
                if (!string.IsNullOrWhiteSpace(entry.Key) && !string.IsNullOrWhiteSpace(entry.Value))
                {
                    sb.Append(entry.Key);
                    sb.Append("=\"");
                    sb.Append(entry.Value);
                    sb.Append("\",");
                }
            }

            return sb.ToString().Substring(0, sb.Length - 1);
        }
    }
}

       
    