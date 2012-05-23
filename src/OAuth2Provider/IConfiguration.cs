using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth2Provider
{
    public interface IConfiguration
    {
        int AccessTokenExpirationLength { get; }
        int AuthorizationTokenExpirationLength { get; }
    }
}
