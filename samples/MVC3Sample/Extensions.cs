using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrackerJack.OAuth.Response;

namespace MVC3Sample
{
    public static class Extensions
    {
        public static OAuthResult OAuth(this IController controller, OAuthResponse oauthResponse)
        {
            return new OAuthResult(oauthResponse);
        }
    }
}