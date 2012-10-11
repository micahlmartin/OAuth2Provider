using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OAuth2Provider.Repository;
using OAuth2Provider.Issuer;
using OAuth2Provider.Request;
using log4net;

namespace OAuth2Provider.Authorization
{
    public class PasswordTokenRequestAuthorizer : IAuthorizeTokenRequest
    {
        private readonly IOAuthServiceLocator _serviceLocator;
        private readonly ILog _logger = LogManager.GetLogger(typeof(PasswordTokenRequestAuthorizer));

        public PasswordTokenRequestAuthorizer(IOAuthServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public Token Authorize(IOAuthRequest request)
        {
            _logger.Debug("Authorizing password token request");

            if (request.ContentType != ContentType.FormEncoded)
                throw new OAuthException(ErrorCode.InvalidRequest, "Invalid content type.");

            //Make sure consumer is valid
            var consumer = _serviceLocator.ConsumerRepository.GetByClientId(request.ClientId);
            if (consumer == null)
                throw new OAuthException(ErrorCode.InvalidClient, "Client credentials are invalid");

            if (consumer.Secret != request.ClientSecret)
                throw new OAuthException(ErrorCode.InvalidClient, "User credentials are invalid");

            //Make sure resource owner is valid
            var resourceOwner = _serviceLocator.ResourceOwnerRepository.GetByUsername(consumer.ConsumerId, request.Username);
            if (resourceOwner == null)
                throw new OAuthException(ErrorCode.InvalidClient, "User credentials are invalid");

            if (!_serviceLocator.PasswordHasher.CheckPassword(request.Password, resourceOwner.Password))
                throw new OAuthException(ErrorCode.InvalidClient, "User credentials are invalid");

            //Make sure consumer is approved by resource owner
            _serviceLocator.ResourceOwnerRepository.ApproveConsumer(resourceOwner.ResourceOwnerId, consumer.ConsumerId);

            var data = new TokenData
            {
                ConsumerId = consumer.ConsumerId,
                ResourceOwnerId = resourceOwner.ResourceOwnerId,
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
