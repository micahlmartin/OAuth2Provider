using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth2Provider.Tests
{
    public class ResourceOwnerImpl : IResourceOwner
    {
        private IList<long> _approvedConsumers;

        public string Username { get; set; }
        public string Password { get; set; }
        public long ResourceOwnerId { get; set; }

        public IEnumerable<long> ApprovedConsumers
        {
            get
            {
                if (_approvedConsumers == null)
                    _approvedConsumers = new List<long>();

                return _approvedConsumers;
            }
            set
            {
                if (value == null) return;

                _approvedConsumers = new List<long>(value);
            }
        }
        public void ApproveConsumer(long consumerId)
        {
            if (!ApprovedConsumers.Contains(consumerId))
                _approvedConsumers.Add(consumerId);
        }
        public void RevokeConsumer(long consumerId)
        {
            if (ApprovedConsumers.Contains(consumerId))
                _approvedConsumers.Remove(consumerId);
        }
        public bool IsConsumerApproved(long consumerId)
        {
            return ApprovedConsumers.Contains(consumerId);
        }
    }
}
