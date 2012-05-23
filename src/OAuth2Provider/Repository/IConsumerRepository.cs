using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth2Provider.Repository
{
    public interface IConsumerRepository
    {
        IConsumer GetByClientId(string clientId);
        IConsumer GetByConsumerId(long consumerId);
    }
}
