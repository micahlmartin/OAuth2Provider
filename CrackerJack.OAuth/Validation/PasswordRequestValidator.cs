using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrackerJack.OAuth.Request;
using log4net;

namespace CrackerJack.OAuth.Validation
{
    public class PasswordRequestValidator : IRequestValidator
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(PasswordRequestValidator));

        public ValidationResult ValidateRequest(IOAuthRequest request)
        {
            _logger.Debug("Validating password request");

            var grantType = request.GrantType;
            if (string.IsNullOrWhiteSpace(grantType))
                return new ValidationResult { ErrorCode = ErrorCode.InvalidRequest, ErrorDescription = "Parameter grant_type is missing" };

            if (grantType != GrantType.Password)
                return new ValidationResult { ErrorCode = ErrorCode.InvalidGrant, ErrorDescription = "The specified grant_type is not supported" };

            var username = request.Username;
            if (string.IsNullOrWhiteSpace(username))
                return new ValidationResult { ErrorCode = ErrorCode.InvalidRequest, ErrorDescription = "Parameter username is missing" };

            var password = request.Password;
            if (string.IsNullOrWhiteSpace(password))
                return new ValidationResult { ErrorCode = ErrorCode.InvalidRequest, ErrorDescription = "Parameter password is missing" };

            var clientId = request.ClientId;
            if (string.IsNullOrWhiteSpace(clientId))
                return new ValidationResult { ErrorCode = ErrorCode.InvalidRequest, ErrorDescription = "Parameter client_id is missing" };

            var clientSecret = request.ClientSecret;
            if (string.IsNullOrWhiteSpace(clientSecret))
                return new ValidationResult { ErrorCode = ErrorCode.InvalidRequest, ErrorDescription = "Parameter client_secret is missing" };

            var contentType = request.ContentType;
            if (contentType != ContentType.FormEncoded)
                return new ValidationResult { ErrorCode = ErrorCode.InvalidRequest, ErrorDescription = "Content-Type header is missing or incorrect." };

            return new ValidationResult { Success = true };
        }
    }
}
