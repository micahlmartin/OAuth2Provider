using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth2Provider.Tests
{
    public class ConsumerImpl : IConsumer
    {
        public string ClientId { get; set; }
        public string Secret { get; set; }
        public long ConsumerId { get; set; }
        public string ApplicationName { get; set; }
        public string Domain { get; set; }
        public string RedirectUrl { get; set; }
        public bool IsEnabled { get; set; }
    }
}
