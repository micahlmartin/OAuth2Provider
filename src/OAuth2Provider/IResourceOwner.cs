using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth2Provider
{
    public interface IResourceOwner
    {
        string Username { get; }
        string Password { get; }
        long ResourceOwnerId { get; }
        IEnumerable<long> ApprovedConsumers { get; }

        void ApproveConsumer(long consumerId);
        void RevokeConsumer(long consumerId);
        bool IsConsumerApproved(long consumerId);
    }
}
