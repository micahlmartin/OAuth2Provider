using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using OAuth2Provider.Validation;
using OAuth2Provider.Authorization;

namespace OAuth2Provider.Request
{
    public class TokenRequest : OAuthRequestBase
    {
        public TokenRequest(IRequest request, IOAuthServiceLocator serviceLocator) : base(request, serviceLocator) { }

        protected override IRequestValidator GetRequestValidator()
        {
            Logger.Debug("Fetching request validator");

            var grantType = GrantType;
            if (string.IsNullOrWhiteSpace(grantType))
                throw new OAuthException(ErrorCode.InvalidRequest, "Parameter grant_type is missing");

            switch (GrantType)
            {
                case OAuth2Provider.GrantType.Password:
                    return new PasswordRequestValidator();
                case OAuth2Provider.GrantType.RefreshToken:
                    return new RefreshTokenRequestValidator();
                case OAuth2Provider.GrantType.AuthorizationCode:
                    return new AuthorizationCodeRequestValidator();
                case OAuth2Provider.GrantType.ClientCredentials:
                    return new ClientCredentialsRequestValidator();
                default:
                    throw new OAuthException(ErrorCode.UnsupportedGrantType, "The grant_type specified is not supported");
            }
            
        }

        public Token Authorize()
        {
            IAuthorizeTokenRequest authorizer;

            switch (GrantType)
            {
                case OAuth2Provider.GrantType.RefreshToken:
                    authorizer = new RefreshTokenRequestAuthorizer(ServiceLocator.ConsumerRepository, ServiceLocator.ResourceOwnerRepository, ServiceLocator.Issuer, ServiceLocator.Configuration);
                    break;
                case OAuth2Provider.GrantType.Password:
                    authorizer = new PasswordTokenRequestAuthorizer(ServiceLocator);
                    break;
                case OAuth2Provider.GrantType.AuthorizationCode:
                    authorizer = new AuthorizationCodeAuthorizer(ServiceLocator.Issuer, ServiceLocator.Configuration);
                    break;
                case OAuth2Provider.GrantType.ClientCredentials:
                    authorizer = new ClientCredentialsTokenRequestAuthorizer(ServiceLocator);
                    break;
                default:
                    throw new OAuthException(ErrorCode.UnsupportedGrantType, "The authorization grant type is not supported");
            }

            return authorizer.Authorize(this);
        }
    }
}
