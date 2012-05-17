using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrackerJack.OAuth.Request;

namespace CrackerJack.OAuth.Authorization
{
    public interface IAuthorizeAuthorizationRequest
    {
        Token Authorize(IOAuthRequest request, long resourceOwnerId);
        bool IsAuthorized(IOAuthRequest request, long resourceOwnerId);
    }
}
