using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Collections.Generic;
using System.Reflection;
using OAuth2Provider.Request;
using Newtonsoft.Json;

namespace OAuth2Provider
{
    public static class Extensions
    {
        public static IDictionary<string, string> ToDictionary(this NameValueCollection nvc)
        {
            return nvc.AllKeys.ToDictionary(key => key, key => nvc[key]);
        }

        public static IDictionary<string, string> ToDictionary(this HttpCookieCollection cookies)
        {
            return cookies.Cast<HttpCookie>().ToDictionary(cookie => cookie.Name, cookie => cookie.Value);
        }

        public static string ToQueryString(this NameValueCollection nvc)
        {
            var sb = new StringBuilder();

            var index = 0;
            foreach (string key in nvc.Keys)
            {
                var val = nvc[key];

                if (index == 0)
                    sb.Append("?");
                else
                    sb.Append("&");

                sb.AppendFormat("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(val));

                index++;
            }

            return sb.ToString();
        }

        public static string ToQueryString(this IDictionary<string, object> dictionary)
        {
            var sb = new StringBuilder();

            var index = 0;
            foreach (var kvp in dictionary)
            {
                if (kvp.Key == null || kvp.Value == null || String.IsNullOrWhiteSpace(kvp.Key) || String.IsNullOrWhiteSpace(kvp.Value.ToString()))
                    continue;

                if (index == 0)
                    sb.Append("?");
                else
                    sb.Append("&");

                sb.AppendFormat("{0}={1}", HttpUtility.UrlEncode(kvp.Key), HttpUtility.UrlEncode(kvp.Value.ToString()));

                index++;
            }

            return sb.ToString();
        }

        public static string RemoveAllDuplicateWhiteSpace(this string value)
        {
            if (value == null)
                return value;

            var val = value.Trim();

            while (val.Contains("  "))
            {
                val = val.Replace("  ", " ");
            }

            return val;
        }

        public static bool HasValue(this string value)
        {
            return !String.IsNullOrWhiteSpace(value);
        }

        public static string GetResponseString(this WebResponse response)
        {
            if (response == null)
                throw new ArgumentNullException("response");

            using (var stream = response.GetResponseStream())
            {
                if (stream == null)
                    throw new ArgumentException("Response stream is null");

                using (var sr = new StreamReader(stream))
                {
                    return sr.ReadToEnd();
                }
            }
        }
        public static string GetResponseString(this WebRequest request)
        {
            using (var response = request.GetResponse())
            {
                return GetResponseString(response);
            }
        }

        public static string TryGetResponseString(this WebRequest request)
        {
            try
            {
                using (var response = request.GetResponse())
                {
                    return GetResponseString(response);
                }
            }
            catch (WebException ex)
            {
                return ex.Response.GetResponseString();
            }
        }

        public static HttpWebResponse TryGetResponse(this WebRequest request)
        {
            try
            {
                return request.GetResponse() as HttpWebResponse;
            }
            catch (WebException ex)
            {
                return ex.Response as HttpWebResponse;
            }
        }

        public static string ToHash(this string value)
        {
            var encoder = new UTF8Encoding();
            var md5Hasher = new MD5CryptoServiceProvider();
            var hashedDataBytes = md5Hasher.ComputeHash(encoder.GetBytes(value));
            return Convert.ToBase64String(hashedDataBytes);
        }

        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static HttpBasicAuthenticationScheme DecodeBasicAuthentication(this IRequest request)
        {
            return new HttpBasicAuthenticationScheme(request);
        }
    }
}
