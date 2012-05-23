using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using OAuth2Provider.Authorization;
using OAuth2Provider.Validation;

namespace OAuth2Provider.Request
{
    public class ResourceRequest : OAuthRequestBase
    {
        public ResourceRequest(IRequest request, IOAuthServiceLocator serviceLocator) : base(request, serviceLocator) { }

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
