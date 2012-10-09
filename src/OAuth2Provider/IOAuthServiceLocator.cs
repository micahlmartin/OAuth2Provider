using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OAuth2Provider.Repository;
using OAuth2Provider.Issuer;

namespace OAuth2Provider
{
    public interface IOAuthServiceLocator
    {
        IConsumerRepository ConsumerRepository { get; }
        IResourceOwnerRepository ResourceOwnerRepository { get; }
        IOAuthIssuer Issuer { get; }
        IConfiguration Configuration { get; }
        IPasswordHasher PasswordHasher { get; }
    }
}
