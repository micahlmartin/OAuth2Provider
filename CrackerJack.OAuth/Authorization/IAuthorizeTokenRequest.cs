using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrackerJack.OAuth.Request;

namespace CrackerJack.OAuth.Authorization
{
    public interface IAuthorizeTokenRequest
    {
        Token Authorize(IOAuthRequest request);
    }
}
