using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OAuth2Provider;
using OAuth2Provider.Request;
using OAuth2Provider.Response;

namespace MVC3Sample.Controllers
{
    public class OAuthController : Controller
    {
        //
        // GET: /OAuth/

        public ActionResult Token()
        {
            try
            {
                var oauthRequest = new TokenRequest(new HttpRequestBaseRequest(Request), MvcApplication.ServiceLocator);

                var token = oauthRequest.Authorize();

                if (token.RedirectsUri.HasValue())
                {

                    var redirectUri = OAuthResponse
                        .TokenResponse(token.AccessToken, token.ExpiresIn, token.RefreshToken)
                        .SetLocation(token.RedirectsUri)
                        .BuildQueryMessage().LocationUri;

                    return Redirect(redirectUri);
                }

                var response = OAuthResponse
                            .TokenResponse(token.AccessToken, token.ExpiresIn, token.RefreshToken)
                            .BuildJsonMessage();

                return this.OAuth(response);
            }
            catch (OAuthException ex)
            {
                var response = new ErrorResponseBuilder(ex).BuildJsonMessage();
                return this.OAuth(response);
            }
        }
    }
}
