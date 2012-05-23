using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OAuth2Provider.Request;

namespace OAuth2Provider.Authorization
{
    public interface IAuthorizeAuthorizationRequest
    {
        Token Authorize(IOAuthRequest request, long resourceOwnerId);
        bool IsAuthorized(IOAuthRequest request, long resourceOwnerId);
    }
}
