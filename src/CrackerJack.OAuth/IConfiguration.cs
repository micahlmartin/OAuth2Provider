using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrackerJack.OAuth
{
    public interface IConfiguration
    {
        int AccessTokenExpirationLength { get; }
        int AuthorizationTokenExpirationLength { get; }
    }
}
