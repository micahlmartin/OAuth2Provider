using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OAuth2Provider.Request;

namespace OAuth2Provider.Validation
{
    public interface IRequestValidator
    {
        ValidationResult ValidateRequest(IOAuthRequest request);
    }
}
