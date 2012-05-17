using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrackerJack.OAuth.Repository
{
    public interface IResourceOwnerRepository
    {
        IResourceOwner GetByUsername(long consumerID, string username);
        IResourceOwner GetByResourceOwnerId(long consumerID, long resourceOwnerId);
        void ApproveConsumer(long resourceOwnerId, long consumerId);
        bool IsConsumerApproved(long resourceOwnerId, long consumerId);
    }
}
