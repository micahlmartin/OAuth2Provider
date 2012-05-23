using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OAuth2Provider.Issuer;
using OAuth2Provider.Repository;
using log4net;

namespace OAuth2Provider.Authorization
{
    public class AuthorizationCodeAuthorizer : IAuthorizeTokenRequest
    {
        private readonly IOAuthIssuer _issuer;
        private readonly IConfiguration _configuration;
        private readonly ILog _logger = LogManager.GetLogger(typeof(AuthorizationCodeAuthorizer));

        public AuthorizationCodeAuthorizer(IOAuthIssuer issuer, IConfiguration configuration)
        {
            _issuer = issuer;
            _configuration = configuration;
        }

        public Token Authorize(Request.IOAuthRequest request)
        {
            _logger.Debug("Authorizing authorization code request");

            var grantType = request.GrantType;
            if (!grantType.HasValue())
                throw new OAuthException(ErrorCode.InvalidRequest, "Parameter grant_type is missing");

            if (request.GrantType != GrantType.AuthorizationCode)
                throw new OAuthException(ErrorCode.InvalidGrant, "The specified grant_type is not supported");

            var code = request.AuthorizationCode;
            if (!code.HasValue())
                throw new OAuthException(ErrorCode.InvalidRequest, "Parameter code is missing");

            var tokenData = _issuer.DecodeAuthorizationToken(code);
            if ((DateTime.UtcNow - new DateTime(tokenData.Timestamp, DateTimeKind.Utc)).TotalSeconds > _configuration.AuthorizationTokenExpirationLength)
                throw new OAuthException(ErrorCode.InvalidRequest, "Authorization code has expired");

            if (tokenData.RedirectUri != request.RedirectUri)
                throw new OAuthException(ErrorCode.InvalidRequest, "The specified redirect_uri is invalid");

            var newTokenData = new TokenData
                                   {
                                       ConsumerId = tokenData.ConsumerId,
                                       ResourceOwnerId = tokenData.ResourceOwnerId,
                                       Timestamp = DateTime.UtcNow.Ticks
                                   };

            return new Token
                       {
                           AccessToken = _issuer.GenerateAccessToken(newTokenData),
                           ExpiresIn = _configuration.AccessTokenExpirationLength,
                           RefreshToken = _issuer.GenerateRefreshToken(newTokenData),
                           RedirectsUri = request.RedirectUri
                       };
        }
    }
}
