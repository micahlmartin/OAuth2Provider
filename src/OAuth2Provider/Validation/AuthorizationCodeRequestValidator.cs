using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace OAuth2Provider.Validation
{
    public class AuthorizationCodeRequestValidator : IRequestValidator
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof (AuthorizationCodeRequestValidator));

        public ValidationResult ValidateRequest(Request.IOAuthRequest request)
        {
            _logger.Debug("Validating authorization code request");

            var grantType = request.GrantType;
            if (string.IsNullOrWhiteSpace(grantType))
                return new ValidationResult { ErrorCode = ErrorCode.InvalidRequest, ErrorDescription = "Parameter grant_type is missing" };

            if (grantType != GrantType.AuthorizationCode)
                return new ValidationResult { ErrorCode = ErrorCode.InvalidGrant, ErrorDescription = "The specified grant_type is not supported" };

            var authCode = request.AuthorizationCode;
            if (string.IsNullOrWhiteSpace(authCode))
                return new ValidationResult { ErrorCode = ErrorCode.InvalidRequest, ErrorDescription = "Parameter authorization_code is missing" };

            return new ValidationResult { Success = true };
        }
    }
}
