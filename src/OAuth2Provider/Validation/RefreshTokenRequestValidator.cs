using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace OAuth2Provider.Validation
{
    public class RefreshTokenRequestValidator :IRequestValidator
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(RefreshTokenRequestValidator));

        public ValidationResult ValidateRequest(Request.IOAuthRequest request)
        {
            _logger.Debug("Validating refresh token request");

            var grantType = request.GrantType;
            if (string.IsNullOrWhiteSpace(grantType))
                return new ValidationResult { ErrorCode = ErrorCode.InvalidRequest, ErrorDescription = "Parameter grant_type is missing" };

            if (grantType != GrantType.RefreshToken)
                return new ValidationResult { ErrorCode = ErrorCode.InvalidGrant, ErrorDescription = "The specified grant_type is not supported" };

            if (!request.IsFormEncoded())
                return new ValidationResult { ErrorCode = ErrorCode.InvalidRequest, ErrorDescription = "Content-Type must be application/x-www-form-urlencoded" };

            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                return new ValidationResult { ErrorCode = ErrorCode.InvalidRequest, ErrorDescription = "Parameter refresh_token is missing" };

            return new ValidationResult {Success = true};
        }
    }
}
