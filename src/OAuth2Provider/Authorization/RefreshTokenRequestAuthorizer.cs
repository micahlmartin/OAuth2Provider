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
    public class RefreshTokenRequestAuthorizer : IAuthorizeTokenRequest
    {        
        private readonly IConsumerRepository _consumerRepository;
        private readonly IResourceOwnerRepository _resourceOwnerRepository;
        private readonly IOAuthIssuer _issuer;
        private readonly IConfiguration _configuration;
        private readonly ILog _logger = LogManager.GetLogger(typeof(RefreshTokenRequestAuthorizer));

        public RefreshTokenRequestAuthorizer(IConsumerRepository consumerRepository, IResourceOwnerRepository resourceOwnerRepository, IOAuthIssuer issuer, IConfiguration configuration)
        {
            _consumerRepository = consumerRepository;
            _resourceOwnerRepository = resourceOwnerRepository;
            _issuer = issuer;
            _configuration = configuration;
        }

        public Token Authorize(IOAuthRequest request)
        {
            _logger.Debug("Authorizing refresh token request");

            //Make sure consumer is valid
            var consumer = _consumerRepository.GetByClientId(request.ClientId);
            if (consumer == null)
                throw new OAuthException(ErrorCode.InvalidClient, "Client credentials are invalid");

            if (consumer.Secret != request.ClientSecret)
                throw new OAuthException(ErrorCode.InvalidClient, "User credentials are invalid");

            var refreshToken = request.RefreshToken;
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new OAuthException(ErrorCode.InvalidRequest, "Refresh token is invalid");

            var tokenData = _issuer.DecodeRefreshToken(refreshToken);

            if (tokenData.ConsumerId != consumer.ConsumerId)
                throw new OAuthException(ErrorCode.UnauthorizedClient, "Refresh token is invalid");

            if (!_resourceOwnerRepository.IsConsumerApproved(tokenData.ResourceOwnerId, tokenData.ConsumerId))
                throw new OAuthException(ErrorCode.UnauthorizedClient, "Unauthorized access");

            var newTokenData = new TokenData
            {
                ConsumerId = consumer.ConsumerId,
                ResourceOwnerId = tokenData.ResourceOwnerId,
                Timestamp = DateTimeOffset.UtcNow.DateTime.Ticks
            };

            return new Token
                       {
                           AccessToken = _issuer.GenerateAccessToken(newTokenData),
                           ExpiresIn = _configuration.AccessTokenExpirationLength,
                           RefreshToken = _issuer.GenerateRefreshToken(newTokenData)
                       };
        }
    }
}
