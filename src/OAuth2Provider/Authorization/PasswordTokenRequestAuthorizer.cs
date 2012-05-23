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
        private readonly IConsumerRepository _consumerRepository;
        private readonly IResourceOwnerRepository _resourceOwnerRepository;
        private readonly IOAuthIssuer _issuer;
        private readonly IConfiguration _configuration;
        private readonly ILog _logger = LogManager.GetLogger(typeof(PasswordTokenRequestAuthorizer));

        public PasswordTokenRequestAuthorizer(IConsumerRepository consumerRepository, IResourceOwnerRepository resourceOwnerRepository, IOAuthIssuer issuer, IConfiguration configuration)
        {
            _consumerRepository = consumerRepository;
            _resourceOwnerRepository = resourceOwnerRepository;
            _issuer = issuer;
            _configuration = configuration;
        }

        public Token Authorize(IOAuthRequest request)
        {
            _logger.Debug("Authorizing password token request");

            if (request.ContentType != ContentType.FormEncoded)
                throw new OAuthException(ErrorCode.InvalidRequest, "Invalid content type.");

            //Make sure consumer is valid
            var consumer = _consumerRepository.GetByClientId(request.ClientId);
            if (consumer == null)
                throw new OAuthException(ErrorCode.InvalidClient, "Client credentials are invalid");

            if (consumer.Secret != request.ClientSecret)
                throw new OAuthException(ErrorCode.InvalidClient, "User credentials are invalid");

            //Make sure resource owner is valid
            var resourceOwner = _resourceOwnerRepository.GetByUsername(consumer.ConsumerId, request.Username);
            if (resourceOwner == null)
                throw new OAuthException(ErrorCode.InvalidClient, "User credentials are invalid");

            if (resourceOwner.Password != request.Password.ToHash())
                throw new OAuthException(ErrorCode.InvalidClient, "User credentials are invalid");

            //Make sure consumer is approved by resource owner
            _resourceOwnerRepository.ApproveConsumer(resourceOwner.ResourceOwnerId, consumer.ConsumerId);

            var data = new TokenData
            {
                ConsumerId = consumer.ConsumerId,
                ResourceOwnerId = resourceOwner.ResourceOwnerId,
                Timestamp = DateTimeOffset.UtcNow.DateTime.Ticks
            };

            return new Token
            {
                AccessToken = _issuer.GenerateAccessToken(data),
                RefreshToken = _issuer.GenerateRefreshToken(data),
                ExpiresIn = _configuration.AccessTokenExpirationLength
            };
        }
    }
}
