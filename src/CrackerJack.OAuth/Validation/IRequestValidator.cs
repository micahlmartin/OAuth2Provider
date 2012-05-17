using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrackerJack.OAuth.Request;

namespace CrackerJack.OAuth.Validation
{
    public interface IRequestValidator
    {
        ValidationResult ValidateRequest(IOAuthRequest request);
    }
}
