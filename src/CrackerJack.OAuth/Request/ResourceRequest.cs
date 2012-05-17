using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using CrackerJack.OAuth.Authorization;
using CrackerJack.OAuth.Validation;

namespace CrackerJack.OAuth.Request
{
    public class ResourceRequest : OAuthRequestBase
    {
        public ResourceRequest(HttpRequestBase request, IOAuthServiceLocator serviceLocator) : base(request, serviceLocator) { }

        protected override IRequestValidator GetRequestValidator()
        {
            return new ResourceRequestValidator();
        }

        public bool Authorize()
        {
            var authorizor = new ResourceRequestAuthorizer(ServiceLocator);

            return authorizor.Authorize(this);
        }
    }
}
