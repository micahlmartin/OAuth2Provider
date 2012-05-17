using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrackerJack.OAuth.Repository;
using CrackerJack.OAuth.Issuer;

namespace CrackerJack.OAuth
{
    public interface IOAuthServiceLocator
    {
        IConsumerRepository ConsumerRepository { get; }
        IResourceOwnerRepository ResourceOwnerRepository { get; }
        IOAuthIssuer Issuer { get; }
        IConfiguration Configuration { get; }
    }
}
