using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OAuth2Provider.Request;
using log4net;

namespace OAuth2Provider.Validation
{
    public class ResourceRequestValidator : IRequestValidator
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(ResourceRequestValidator));

        public ValidationResult ValidateRequest(IOAuthRequest request)
        {
            _logger.Debug("Validating resource request");

            if (string.IsNullOrWhiteSpace(request.AccessToken))
                return new ValidationResult {Success = false};

            return new ValidationResult {Success = true};
        }
    }
}
