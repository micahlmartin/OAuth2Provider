using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuth2Provider
{
    public interface IOAuthMessageWriter
    {
        void Write(IOAuthMessage message, IDictionary<string, object> parameters);
    }
}
