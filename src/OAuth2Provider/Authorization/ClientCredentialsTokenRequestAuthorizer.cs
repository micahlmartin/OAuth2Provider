using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OAuth2Provider.Issuer;
using OAuth2Provider.Request;
using log4net;

namespace OAuth2Provider.Authorization
{
    public class ClientCredentialsTokenRequestAuthorizer : IAuthorizeTokenRequest
    {
        private readonly IOAuthServiceLocator _serviceLocator;
        private readonly ILog _logger = LogManager.GetLogger(typeof(ClientCredentialsTokenRequestAuthorizer));

        public ClientCredentialsTokenRequestAuthorizer(IOAuthServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public Token Authorize(IOAuthRequest request)
        {
            _logger.Debug("Authorizing client credentials token request");

            //Make sure consumer is valid
            var consumer = _serviceLocator.ConsumerRepository.GetByClientId(request.ClientId);
            if (consumer == null || !consumer.Secret.Equals(request.ClientSecret,StringComparison.OrdinalIgnoreCase))
                throw new OAuthException(ErrorCode.InvalidClient, "Client credentials are invalid");

            var data = new TokenData
            {
                ConsumerId = consumer.ConsumerId,
                Timestamp = DateTimeOffset.UtcNow.DateTime.Ticks
            };

            return new Token
            {
                AccessToken = _serviceLocator.Issuer.GenerateAccessToken(data),
                RefreshToken = _serviceLocator.Issuer.GenerateRefreshToken(data),
                ExpiresIn = _serviceLocator.Configuration.AccessTokenExpirationLength
            };
        }
    }
}
