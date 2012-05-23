using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace OAuth2Provider.Request
{
    public interface IOAuthRequest
    {
        string GrantType { get; }
        string Username { get; }
        string Password { get; }
        string Scope { get; }
        string AccessToken { get; }
        string TokenType { get; }
        string ExpiresIn { get; }
        string RefreshToken { get; }
        string ResponseType { get; }
        string RedirectUri { get; }
        string State { get; }
        string ClientId { get; }
        string ClientSecret { get; }
        string ContentType { get; }
        string AuthorizationCode { get; }
        string Method { get; }
        bool IsSecure { get; }
    }
}
