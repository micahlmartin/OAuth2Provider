using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth2Provider.Issuer
{
    public class TokenData
    {
        public long ConsumerId { get; set; }
        public long ResourceOwnerId { get; set; }
        public long Timestamp { get; set; }
        public string RedirectUri { get; set; }
    }
}
