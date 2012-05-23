using System;
using System.Text;
using System.Web;
using OAuth2Provider.Request;

namespace OAuth2Provider
{
    public class HttpBasicAuthenticationScheme
    {
        public HttpBasicAuthenticationScheme(IRequest request)
        {
            try
            {
                var header = (request.Headers["Authorization"] + "").Replace("Basic", "").Trim();
                var tokens = Encoding.ASCII.GetString(Convert.FromBase64String(header)).Split(new[] { ':' });
                if (tokens.Length > 0)
                    Username = tokens[0];
                if (tokens.Length > 1)
                    Password = tokens[1];
            }
            catch (Exception) { }

        }

        public HttpBasicAuthenticationScheme(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public string Username { get; set; }
        public string Password { get; set; }

        public override string ToString()
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", Username, Password)));
        }
    }
}
