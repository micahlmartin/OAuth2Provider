using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrackerJack.OAuth;
using CrackerJack.OAuth.Issuer;
using CrackerJack.OAuth.Request;

namespace MVC3Sample.Controllers
{
    public class SecureController : Controller
    {
        //
        // GET: /Secure/

        protected TokenData TokenData { get; private set; }
        private readonly IOAuthServiceLocator _serviceLocator;

        public SecureController()
        {
            _serviceLocator = MvcApplication.ServiceLocator;
        }

        public ActionResult Index(int id)
        {
            var resourceOwner = _serviceLocator.ResourceOwnerRepository.GetByResourceOwnerId(TokenData.ConsumerId, id);
            return Json(resourceOwner, JsonRequestBehavior.AllowGet);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            AuthorizeRequest(filterContext);
        }

        protected virtual void AuthorizeRequest(ActionExecutingContext filterContext)
        {
            var isAuthorized = false;

            try
            {
                var resourceRequest = new ResourceRequest(Request, _serviceLocator);
                isAuthorized = resourceRequest.Authorize();

                TokenData = _serviceLocator.Issuer.DecodeAccessToken(resourceRequest.AccessToken);
            }
            catch (OAuthException)
            {
            }

            if (isAuthorized)
                return;

            throw new UnauthorizedAccessException();
        }
    }
}
