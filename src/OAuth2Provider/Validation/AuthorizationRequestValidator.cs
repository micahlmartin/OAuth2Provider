using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace OAuth2Provider.Validation
{
    public class AuthorizationRequestValidator : IRequestValidator
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(AuthorizationRequestValidator));

        public ValidationResult ValidateRequest(Request.IOAuthRequest request)
        {
            _logger.Debug("Validating authorization request");

            var responseType = request.ResponseType;
            if (string.IsNullOrWhiteSpace(request.ResponseType))
                return new ValidationResult { ErrorCode = ErrorCode.InvalidRequest, ErrorDescription = "Parameter response_type is missing" };

            if(responseType != ResponseType.Code)
                return new ValidationResult { ErrorCode = ErrorCode.InvalidRequest, ErrorDescription = "The specified response_type is not valid" };

            var clientId = request.ClientId;
            if (string.IsNullOrWhiteSpace(clientId))
                return new ValidationResult { ErrorCode = ErrorCode.InvalidRequest, ErrorDescription = "Parameter client_id is missing" };

            var redirectUri = request.RedirectUri;
            if (redirectUri != null && !Regexs.HTTPUrl.IsMatch(redirectUri))
                return new ValidationResult { ErrorCode = ErrorCode.InvalidRequest, ErrorDescription = "The redirect_url parameter must be an absolute URI" };

            return new ValidationResult {Success = true};
        }
    }
}
