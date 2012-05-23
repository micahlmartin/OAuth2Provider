using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using OAuth2Provider.Authorization;
using OAuth2Provider.Issuer;
using OAuth2Provider.Validation;

namespace OAuth2Provider.Request
{
    public class AuthorizationRequest : OAuthRequestBase
    {
        public AuthorizationRequest(IRequest request, IOAuthServiceLocator serviceLocator) : base(request, serviceLocator) { }

        protected override IRequestValidator GetRequestValidator()
        {
            return new AuthorizationRequestValidator();
        }

        public string GetRedirectUri(IConsumer consumer)
        {
            var redirectUri = RedirectUri;
            if (redirectUri != null)
            {
                var consumerDomain = consumer.Domain.ToLowerInvariant();
                var match = Regexs.HTTPUrl.Match(redirectUri);
                if (!match.Success)
                    throw new OAuthException(ErrorCode.InvalidRequest, "The redirect_uri parameter must be an absolute uri");

                var domain = match.Groups["domain"].Value.ToLowerInvariant();
                var subdomain = match.Groups["subdomain"].Value.ToLowerInvariant();

                string redirectDomain;
                if (domain.Count(x => x == '.') == 1)
                    redirectDomain = subdomain + domain;
                else
                    redirectDomain = domain.Trim(new[] { '.' });

                if (redirectDomain != consumerDomain)
                    throw new OAuthException(ErrorCode.InvalidRequest, "The redirect_uri is not authorized");
            }
            else
                redirectUri = consumer.RedirectUrl;

            return redirectUri;
        }

        public Token GetAuthorizationToken(long consumerId, long resourceOwnerId, string redirectUri)
        {
            var data = new TokenData
            {
                ConsumerId = consumerId,
                Timestamp = DateTimeOffset.UtcNow.DateTime.Ticks,
                ResourceOwnerId = resourceOwnerId,
                RedirectUri = redirectUri
            };

            return new Token
            {
                AuthorizationToken = ServiceLocator.Issuer.GenerateAuthorizationToken(data),
                ExpiresIn = ServiceLocator.Configuration.AuthorizationTokenExpirationLength
            };
        }
    }
}
