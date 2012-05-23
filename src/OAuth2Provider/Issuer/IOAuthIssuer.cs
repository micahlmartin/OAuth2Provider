using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth2Provider.Issuer
{
    public interface IOAuthIssuer
    {
        string GenerateAccessToken(TokenData data);
        string GenerateRefreshToken(TokenData data);
        string GenerateAuthorizationToken(TokenData data);

        TokenData DecodeAccessToken(string token);
        TokenData DecodeRefreshToken(string token);
        TokenData DecodeAuthorizationToken(string data);
    }
}
