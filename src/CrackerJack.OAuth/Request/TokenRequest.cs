using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using CrackerJack.OAuth.Validation;
using CrackerJack.OAuth.Authorization;

namespace CrackerJack.OAuth.Request
{
    public class TokenRequest : OAuthRequestBase
    {
        public TokenRequest(HttpRequestBase request, IOAuthServiceLocator serviceLocator) : base(request, serviceLocator) { }

        protected override IRequestValidator GetRequestValidator()
        {
            Logger.Debug("Fetching request validator");

            var grantType = GrantType;
            if (string.IsNullOrWhiteSpace(grantType))
                throw new OAuthException(ErrorCode.InvalidRequest, "Parameter grant_type is missing");

            switch (GrantType)
            {
                case OAuth.GrantType.Password:
                    return new PasswordRequestValidator();
                case OAuth.GrantType.RefreshToken:
                    return new RefreshTokenRequestValidator();
                case OAuth.GrantType.AuthorizationCode:
                    return new AuthorizationCodeRequestValidator();
                case OAuth.GrantType.ClientCredentials:
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
                case OAuth.GrantType.RefreshToken:
                    authorizer = new RefreshTokenRequestAuthorizer(ServiceLocator.ConsumerRepository, ServiceLocator.ResourceOwnerRepository, ServiceLocator.Issuer, ServiceLocator.Configuration);
                    break;
                case OAuth.GrantType.Password:
                    authorizer = new PasswordTokenRequestAuthorizer(ServiceLocator.ConsumerRepository, ServiceLocator.ResourceOwnerRepository, ServiceLocator.Issuer, ServiceLocator.Configuration);
                    break;
                case OAuth.GrantType.AuthorizationCode:
                    authorizer = new AuthorizationCodeAuthorizer(ServiceLocator.Issuer, ServiceLocator.Configuration);
                    break;
                case OAuth.GrantType.ClientCredentials:
                    authorizer = new ClientCredentialsTokenRequestAuthorizer(ServiceLocator);
                    break;
                default:
                    throw new OAuthException(ErrorCode.UnsupportedGrantType, "The authorization grant type is not supported");
            }

            return authorizer.Authorize(this);
        }
    }
}
