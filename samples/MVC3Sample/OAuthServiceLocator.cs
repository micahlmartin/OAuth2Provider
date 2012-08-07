using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OAuth2Provider;
using OAuth2Provider.Issuer;
using OAuth2Provider.Repository;

namespace MVC3Sample
{
    public class OAuthServiceLocator : IOAuthServiceLocator
    {
        private readonly IConsumerRepository _consumerRepository = new ConsumerRepository();
        private readonly IResourceOwnerRepository _resourceOwnerRepository = new ResourceOwnerRepository();
        private readonly IOAuthIssuer _oAuthIssuer = new OAuthIssuer();
        private readonly IConfiguration _configuration = new Configuration();

        public IConsumerRepository ConsumerRepository
        {
            get { return _consumerRepository; }
        }

        public IResourceOwnerRepository ResourceOwnerRepository
        {
            get { return _resourceOwnerRepository; }
        }

        public IOAuthIssuer Issuer
        {
            get { return _oAuthIssuer; }
        }

        public IConfiguration Configuration
        {
            get { return _configuration; }
        }
    }

    public class ConsumerRepository : IConsumerRepository
    {
        private readonly IList<Consumer> _consumers = new List<Consumer>
                                                          {
                                                              new Consumer
                                                                  {
                                                                      ApplicationName = "My App",
                                                                      ClientId = "6E470F52-293D-4BDD-92D2-069006E50A9F",
                                                                      ConsumerId = 1,
                                                                      Domain = "mydomain.com",
                                                                      IsEnabled = true,
                                                                      RedirectUrl = "http://mydomain.com/auth",
                                                                      Secret = "FB847641-8EF2-481C-801A-0BF617BBCA6C"
                                                                  }
                                                          };
 
        public IConsumer GetByClientId(string clientId)
        {
            return _consumers.FirstOrDefault(x => x.ClientId == clientId);
        }

        public IConsumer GetByConsumerId(long consumerId)
        {
            return _consumers.FirstOrDefault(x => x.ConsumerId == consumerId);
        }
    }

    public class ResourceOwnerRepository : IResourceOwnerRepository
    {
        private readonly IList<ResourceOwner> _resourceOwners = new List<ResourceOwner>
                                                                    {
                                                                        new ResourceOwner
                                                                            {
                                                                                ApprovedConsumers = new List<long>{1},
                                                                                Password = "mypassword",
                                                                                ResourceOwnerId = 1,
                                                                                Username = "myusername",
                                                                            }
                                                                    };

        public IResourceOwner GetByUsername(long consumerID, string username)
        {
            return _resourceOwners.FirstOrDefault(x => x.Username.ToLower() == username.ToLower());

            //Can optionally segregate users by consumer in a multi-tenant environment 
            // return _resourceOwners.FirstOrDefault(x => x.Username.ToLower() == username.ToLower() && x.ConsumerID == consumerID);
        }

        public IResourceOwner GetByResourceOwnerId(long consumerID, long resourceOwnerId)
        {
            return _resourceOwners.FirstOrDefault(x => x.ResourceOwnerId == resourceOwnerId);

            //Can optionally segregate users by consumer in a multi-tenant environment
            // return _resourceOwners.FirstOrDefault(x => x.ResourceOwnerId == resourceOwnerId && x.ConsumerID == consumerID);
        }

        public void ApproveConsumer(long resourceOwnerId, long consumerId)
        {
            var resourceOwner = _resourceOwners.FirstOrDefault(x => x.ResourceOwnerId == resourceOwnerId);
            if (resourceOwner != null)
                resourceOwner.ApproveConsumer(resourceOwnerId);
        }

        public bool IsConsumerApproved(long resourceOwnerId, long consumerId)
        {
            var resourceOwner = _resourceOwners.FirstOrDefault(x => x.ResourceOwnerId == resourceOwnerId);
            if (resourceOwner != null)
                return resourceOwner.IsConsumerApproved(consumerId);

            return false;
        }
    }

    public class Configuration : IConfiguration
    {
        public int AccessTokenExpirationLength
        {
            get { return 30; }
        }

        public int AuthorizationTokenExpirationLength
        {
            get { return 30; }
        }
    }

    public class Consumer : IConsumer
    {
        public string ApplicationName { get; set; }
        public string ClientId { get; set; }
        public long ConsumerId { get; set; }
        public string Domain { get; set; }
        public bool IsEnabled { get; set; }
        public string RedirectUrl { get; set; }
        public string Secret { get; set; }
    }

    public class ResourceOwner : IResourceOwner
    {
        public ResourceOwner()
        {
            ApprovedConsumers = new List<long>();
        }

        public void ApproveConsumer(long consumerId)
        {
            if(!ApprovedConsumers.Contains(consumerId))
                ((IList<long>) ApprovedConsumers).Add(consumerId);
        }

        public void RevokeConsumer(long consumerId)
        {
            if (ApprovedConsumers.Contains(consumerId))
                ((IList<long>)ApprovedConsumers).Remove(consumerId);
        }

        public bool IsConsumerApproved(long consumerId)
        {
            return ApprovedConsumers.Contains(consumerId);
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public long ResourceOwnerId { get; set; }
        public IEnumerable<long> ApprovedConsumers { get; set; }
    }
}