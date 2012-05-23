using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OAuth2Provider.Issuer;
using OAuth2Provider.Repository;
using log4net;

namespace OAuth2Provider.Authorization
{
    public class ResourceRequestAuthorizer : IAuthorizeResourceRequest
    {
        private readonly IConsumerRepository _consumerRepository;
        private readonly IResourceOwnerRepository _resourceOwnerRepository;
        private readonly IOAuthIssuer _issuer;
        private readonly IConfiguration _configuration;
        private readonly ILog _logger = LogManager.GetLogger(typeof (ResourceRequestAuthorizer));

        public ResourceRequestAuthorizer(IOAuthServiceLocator serviceLocator)
        {
            _consumerRepository = serviceLocator.ConsumerRepository;
            _resourceOwnerRepository = serviceLocator.ResourceOwnerRepository;
            _issuer = serviceLocator.Issuer;
            _configuration = serviceLocator.Configuration;
        }

        public bool Authorize(Request.IOAuthRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.AccessToken))
                return false;

            var tokenData = _issuer.DecodeAccessToken(request.AccessToken);

            if ((DateTime.UtcNow - new DateTime(tokenData.Timestamp, DateTimeKind.Utc)).TotalSeconds > _configuration.AccessTokenExpirationLength)
            {
                _logger.Info("Access token is expired");
                return false;
            }

            return true;
        }
    }
}
