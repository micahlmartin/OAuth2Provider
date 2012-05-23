using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth2Provider
{
    public interface IConsumer
    {
        string ClientId { get; }
        string Secret { get; }
        long ConsumerId { get; }
        string ApplicationName { get; }
        string Domain { get; }
        string RedirectUrl { get; }
        bool IsEnabled { get; }
    }
}
